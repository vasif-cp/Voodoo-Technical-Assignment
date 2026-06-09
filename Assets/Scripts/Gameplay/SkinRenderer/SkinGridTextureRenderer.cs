using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SkinGridTextureRenderer : MonoBehaviour
{
    [SerializeField] private BrushMainMenu m_SkinGridPanelPrefab;
    [SerializeField] private Transform m_SkinGridContainer;
    
    private IGameService m_GameService;
    
    private static readonly int s_Rows = 4;
    private static readonly int s_Columns = 3;
    private static readonly float s_HeightOffset = 0.05f;
    
    [Inject]
    public void Construct(IGameService gameService)
    {
        m_GameService = gameService;
    }
    
    private void Awake()
    {
        InitializeScene(m_GameService.m_Skins);
    }

    private void InitializeScene(List<SkinData> _AllSkins)
    {
        foreach (var skinData in _AllSkins)
        {
            var skinPanel = Instantiate(m_SkinGridPanelPrefab, m_SkinGridContainer);
            skinPanel.Set(skinData);
        }
    }
    
    public static Rect GetGridUvRect(int index)
    {
        float width = 1f / s_Columns;  
        float height = 1f / s_Rows;

        int column = index % s_Columns;
        int row = index / s_Columns;

        float x = column * width;
        float y = 1f - (row + 1) * height;
        
        return new Rect(x, y, width, height);
    }
}
