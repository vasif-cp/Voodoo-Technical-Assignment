using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterGameModeData", menuName = "GameMode Settings/BoosterGameModeData", order = 1)]
public class BoosterGameModeData : GameModeData
{ 
    [SerializeField] private BoosterModeLevelsData m_LevelsData;
    
    public override GameModeType m_GameModeType => GameModeType.BoosterMode;
    
    private BoosterModeLevel GetCurrentLevel(IStatsService statsService)
    {
        int boosterLevel = GetModeLevel(statsService);
        return m_LevelsData.GetBoosterModeLevelData(boosterLevel);
    }
    
    public override IReadOnlyList<PowerUpData> GetAvailablePowerUps(IStatsService statsService)
    {
        var level = GetCurrentLevel(statsService);
        if (level == null) return s_AllPowerUps;

        return level.m_PowerUps;
    }
    
    public override void OnGameEnd(IStatsService statsService, int playerRank)
    {
        if (playerRank != 0)
            return;

        int currentLevel = GetModeLevel(statsService);
        int nextLevel = currentLevel + 1;

        SetModeLevel(statsService, nextLevel);
    }

}
