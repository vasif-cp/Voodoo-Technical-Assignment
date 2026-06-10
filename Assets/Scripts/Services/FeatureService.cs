using System;
using UnityEngine;

public class FeatureService : IFeatureService
{
    public event Action<GameFeature, bool> OnFeatureChanged;

    private const bool c_DefaultEnabled = true;

    public bool IsEnabled(GameFeature feature) => PlayerPrefs.GetInt(Key(feature), c_DefaultEnabled ? 1 : 0) == 1;

    public void SetEnabled(GameFeature feature, bool enabled)
    {
        if (IsEnabled(feature) == enabled) return;
        PlayerPrefs.SetInt(Key(feature), enabled ? 1 : 0);
        OnFeatureChanged?.Invoke(feature, enabled);
    }

    private string Key(GameFeature feature) => $"Feature_{feature}";
}