using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameModeType {DefaultMode, BoosterMode}

public abstract class GameModeData : ScriptableObject
{
    [Header("Common Game Mode Settings")] 
    [SerializeField] private PowerUpData[] m_DefaultPowerUps;  
    public float m_MinPowerUpRate = 1.5f;
    public float m_MaxPowerUpRate = 2.5f;
    public float m_PopPointTeamPadding = 7.5f;
    public float m_PopPointPadding = 15.0f;
    public float m_PowerUpPadding = 13.0f;
    public float m_BrushRate = 16.0f;
    
    
    public abstract GameModeType m_GameModeType { get; }
    
    public abstract IReadOnlyList<PowerUpData> GetAvailablePowerUps(IStatsService statsService);
    
    public virtual int GetModeLevel(IStatsService statsService)
    {
        return statsService.GetGameModeLevel(m_GameModeType);
    }
    
    public virtual void SetModeLevel(IStatsService statsService, int level)
    {
        statsService.SetGameModeLevel(m_GameModeType, level);
    }
    
    public virtual void OnGameEnd(IStatsService statsService, int playerRank) { }
    
    protected IReadOnlyList<PowerUpData> GetDefaultPowerUps() => m_DefaultPowerUps;

}
