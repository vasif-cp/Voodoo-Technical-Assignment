using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultGameModeData", menuName = "GameMode Settings/DefaultGameModeData", order = 0)]
public class DefaultGameModeData : GameModeData
{
    public override GameModeType m_GameModeType => GameModeType.DefaultMode;
    
    public override IReadOnlyList<PowerUpData> GetAvailablePowerUps(IStatsService statsService)
    {
        return s_AllPowerUps;
    }
}
