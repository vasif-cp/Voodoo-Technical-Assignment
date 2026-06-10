using System;

public enum GameFeature {BoosterGameMode, SkinSelection}

public interface IFeatureService
{
    event Action<GameFeature, bool> OnFeatureChanged;
    bool IsEnabled(GameFeature feature);
    void SetEnabled(GameFeature feature, bool enabled);
}

