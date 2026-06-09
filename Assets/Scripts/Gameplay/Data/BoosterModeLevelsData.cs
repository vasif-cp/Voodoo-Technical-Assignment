using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoosterModeLevel
{
    public int m_Level;
    public List<PowerUpData> m_PowerUps;
}

[CreateAssetMenu(fileName = "BoosterModeLevelsData", menuName = "GameMode Settings/BoosterMode/BoosterModeLevelsData")]
public class BoosterModeLevelsData : ScriptableObject
{
    public List<BoosterModeLevel> m_BoosterModeLevels;
    
    public BoosterModeLevel GetBoosterModeLevelData(int levelID)
    {
        if (m_BoosterModeLevels == null || m_BoosterModeLevels.Count == 0)
            return null;

        return m_BoosterModeLevels.Find(l => l.m_Level == levelID);
    }
}
