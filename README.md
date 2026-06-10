# Voodoo — Technical Assignment

Solutions for Voodoo's case study, built on top of the existing `Draw.io`-style game.
Three features were added — **Booster Game Mode**, **Skin Selection Screen**, and a **Debug Menu** — plus a fix for a rewarded-ad input bug.

All three features are **independent and individually toggleable**. With every feature disabled from the Debug Menu, the game returns to its original behaviour.

---

## A note on AI assistance

This **README** was written with the help of an AI assistant (Claude Opus 4.8) purely to improve its **structure, clarity, and wording**.

**During development, No AI-generated code or project assets were used in the project. All C# code, Unity scenes, prefabs, and ScriptableObject setup were created and assembled by hand.**

---

## Design principles (applied across all features)

- **Fit the existing codebase.** Reused the project's conventions: Zenject services, the `View<T>` / phase-driven UI system, ScriptableObject configs, and `PlayerPrefs` persistence.
- **Data-driven over hard-coded.** Gameplay tuning and content live in ScriptableObjects/assets so designers can iterate without touching code.
- **Extensible by composition.** Adding a new mode, skin, or feature should require new data + a small class, not edits to core systems.
- **Independence.** Each feature is additive and reversible, so it can be switched off cleanly.

---

## Feature 1 — Booster Game Mode

A second game mode launched from the Main Menu, with its **own infinite progression** (separate from the main game) and a **per-level booster setup**.

**How it works**
- `GameModeData` is an abstract `ScriptableObject` (a **Strategy**); `DefaultGameModeData` and `BoosterGameModeData` are concrete modes. `GameService` runs through a single `m_ActiveGameMode`.
- Per-mode tuning (spawn rates, paddings, brush rate, available boosters) lives **on the data**, not in `GameService`.
- Progression is stored **per mode** in `PlayerPrefs` (`GameModeLevel_<mode>`), so the booster mode never touches the main game's progression.
- **Level design** is data: `BoosterModeLevelsData` holds a list of levels, each with its own booster set.
- **Two new boosters** were added for this mode only — **Speed Up** (temporary speed boost) and **Random-Position Paint Bomb** (fills a circle at a random spot in the player's colour). Mode-exclusivity is enforced by **curated per-mode lists** (each mode references exactly the boosters it should have) rather than loading every asset globally.

**Why this was the right call**
- Adding a third mode is trivial and isolated.
- Booster setups are editable by designers, with no code changes.
- Mode-exclusive content is guaranteed by construction, not by accident.

**Example — adding a new mode**
```
1. Create class:  public class TimeAttackGameModeData : GameModeData { ... }
2. Add enum:      enum GameModeType { Default, Booster, TimeAttack }
3. Create asset + add it to GameModeConfig.
   → No changes to GameService.
```

---

## Feature 2 — Skin Selection Screen

A scrollable screen showing **12 skins (2 models × 6 colours)**. Every skin — both the list items and the large preview on top — **rotates slowly**. The player's choice is **saved and restored** between sessions.

**How it works**
- A single **render-texture atlas**: one orthographic camera (in an additive `SkinRenderer` scene) renders all 12 rotating 3D models into **one** RenderTexture arranged as a 3×4 grid.
- Each UI cell is a `RawImage` that shows **its region** of that texture via `uvRect`. The big top preview samples the **same** texture, so it rotates in perfect sync with the grid — for free.
- Rotation reuses the existing `BrushRotation` component on the 3D models.
- The selection is persisted in `PlayerPrefs` (`FavoriteSkin`) as the **index** into the skin list, consistent with how the rest of the game already reads the chosen skin.

**Why this was the right call**
- **Device-friendly:** one camera + one RenderTexture + one scene draw, instead of 12 cameras/textures.
- **Clean 3D-in-UI integration:** the 3D rig is isolated in its own scene; the UI only deals with a texture.
- **Consistent persistence:** the saved value matches the game's existing skin indexing, so the menu, preview, and in-game skin always agree.

**Example — how a cell maps to the atlas**
```
12 models → one 384×640 RenderTexture (3 columns × 4 rows)
Cell(index) → RawImage.uvRect = GetGridUvRect(index)   // samples that skin's tile
```

---

## Feature 3 — Debug Menu

A menu reachable from the Main Menu that lets you **enable/disable each feature independently**. With everything off, the game is in its **original state**.

**How it works**
- `IFeatureService` / `FeatureService` is a Zenject singleton backed by `PlayerPrefs` (`Feature_<name>`), with an `OnFeatureChanged` event.
- The menu is **enum-driven**: `DebugMenuView` iterates the `GameFeature` enum and **auto-generates one toggle per feature**. Adding a feature needs no menu edits.
- `FeatureToggleBinding` declaratively shows/hides each feature's entry points (e.g. show the new skin-shop button when on, restore the original brush arrows when off).
- The menu is intended for development builds and is shown **only on the Main Menu**.

**Why this was the right call**
- **One source of truth** for what's on/off, persisted per device.
- **Zero-touch extensibility:** new feature in the enum → new toggle appears automatically.
- **Declarative gating:** turning a feature off restores the original UI/flow with no special cases.

**Example — adding a toggle**
```
enum GameFeature { BoosterGameMode, SkinSelection, NewFeature }
   → "New Feature" toggle appears in the menu automatically.
```

**Example — original state**
```
Disable Booster + Skin Selection  →  Main Menu shows only the original Play button,
                                      and the base game runs exactly as before.
```

---

## Bonus fix — Rewarded-ad input drift

**Symptom:** after watching a rewarded ad to revive, the player kept drifting forward for a few seconds even though input felt disabled.

**Root cause:** the rewarded ad interrupts the touch and the **touch-release event is never delivered**. The input controller stayed latched in its "still dragging" state and the player kept its last movement vector, so on revive it resumed moving from that **stale input** until the next real touch release.

**Fix:** the input/drag state is **reset whenever gameplay is interrupted** (the "moving" latch is cleared and movement zeroed), and a **fresh touch is required to resume**. This targets the root cause (stale input state) rather than the symptom, so it's robust to any interruption — ads, app focus loss, etc.

---

## Summary

| Feature | Core idea | Key benefit |
|---|---|---|
| Booster Game Mode | Strategy + data-driven modes & levels | Easy to extend, designer-editable, mode-exclusive content |
| Skin Selection | Single render-texture atlas | Optimized 3D-in-UI, synced rotation, consistent persistence |
| Debug Menu | Enum-driven feature flags | Independent toggling, original state restorable |
| Ads input fix | Reset stale input on interruption | Root-cause fix, robust to all interruptions |

---

## Possible improvements (with more time)

The features were built to fit the existing game. With more hours, I would invest in the following.

### 1. Reduce legacy coupling in the core
The project leans on global access patterns that make features harder to isolate and extend:
- `GameService.SetColor` reaches directly into concrete UI singletons (`MainMenuView`, `RVEndView`, `LoadingView`, and now `SkinShopView`) — a core service depending on specific views.
- Heavy use of `SingletonMB.Instance` (backed by `FindObjectOfType`) and static cross-references (e.g. `MainMenuView.Instance.OnPlay(...)`) instead of dependency injection throughout.
- Content is loaded with string-based `Resources.LoadAll(...)` (skins, power-ups, brushes, colours).

**What I'd do:** route these through events/interfaces and DI — for example a single `onPlayerColorChanged` event that the views subscribe to instead of `SetColor` calling each view — and migrate `Resources` loading to explicit references or Addressables. This makes every feature fully independent and lowers the risk of cross-feature regressions.

### 2. Establish a single, enforced code standard
I matched the local convention case-by-case, but the project has **no single standard**, so "match the surrounding code" is often ambiguous:
- Inconsistent member naming — some properties use `m_` (`m_IsPlaying`), others camelCase (`currentPhase`, `isEliminated`), others PascalCase (`WorldHalfWidth`).
- Mixed parameter styles (`_Player` vs `player`) and a mix of English/French comments.
- No `.editorconfig` or analyzer enforcing rules; member ordering is ad-hoc.

**What I'd do:** add an `.editorconfig` (plus a Roslyn analyzer / StyleCop) and a short contributing note, then run a formatting pass. Consistency speeds up review and onboarding and removes guesswork when extending the code.

### 3. Make the Random-Position Paint Bomb read clearly (UX)
Functionally it fills a circle in the player's colour at a random position on the map, but the **cause → effect is not obvious**: the fill appears away from the player with no telegraph, so it isn't clear what happened or that it benefited the player.

**What I'd do:** add clear feedback — a short projectile/arc or trail from the player to the target, a target marker/ring just before the fill, and a matching VFX/SFX, with the fill unmistakably in the player's colour. This keeps the mechanic legible and improves game feel without changing the underlying logic.
