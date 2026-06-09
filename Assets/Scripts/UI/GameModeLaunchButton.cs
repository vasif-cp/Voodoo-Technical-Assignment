using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameModeLaunchButton : MonoBehaviour
{
    [SerializeField] private GameModeType m_GameModeType;
    [SerializeField] private TMP_Text m_LevelLabel;
    
    private IStatsService m_StatsService;

    [Inject]
    public void Construct(IStatsService statsService)
    {
        m_StatsService = statsService;
    }
    
    private void Start()
    {
        Button button =  GetComponent<Button>();
        button.onClick.AddListener(LaunchGameMode);
        if (m_LevelLabel != null)
        {
            m_LevelLabel.text = $"Lvl {m_StatsService.GetGameModeLevel(m_GameModeType)}";
        }
    }

    private void LaunchGameMode()
    {
        MainMenuView.Instance.OnPlay(m_GameModeType);
    }
}
