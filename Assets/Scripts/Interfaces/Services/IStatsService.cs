using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatsService
{
    public float GetLevel();
    void TryToSetBestScore(int playerScore);
    void AddGameResult(int rankingScore);
    void SetLastXP(int xp);
    void GainXP();
    int FavoriteSkin { get; set; }
    int m_LastGain { get; set; }
    int GetXP();
    int GetPlayerLevel();
    int XPToNextLevel(int currentLevel);
    string GetNickname();
    void SetNickname(string name);
    int GetGameModeLevel(GameModeType gameModeType);
    void SetGameModeLevel(GameModeType gameModeType, int level);
}
