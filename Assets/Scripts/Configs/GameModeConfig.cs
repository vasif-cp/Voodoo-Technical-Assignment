using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameModeConfig", menuName = "Config/GameModeConfig")]
public class GameModeConfig : ScriptableObject
{
    public GameModeData[] m_GameModeConfigs;
    
    public GameModeData GetGameModeData(GameModeType gameModeType)
    {
        var selectedGameMode =  m_GameModeConfigs.FirstOrDefault(gameModeConfig => gameModeConfig.m_GameModeType == gameModeType);
        return selectedGameMode != null ? selectedGameMode : m_GameModeConfigs[0];
    }
}
