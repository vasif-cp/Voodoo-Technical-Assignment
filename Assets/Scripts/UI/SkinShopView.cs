using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class SkinShopView : View<SkinShopView>
{
    public SkinViewPanel m_SkinViewPanelPrefab;
    public RawImage m_SelectedBrushRenderer;
    public RectTransform m_SkinViewsContainer;
    public List<Image> m_ColoredImages;
    
    private IStatsService m_StatsService;
    private SkinViewPanel[] m_SkinViewPanels;
    
    private readonly WaitForSeconds m_AnimWaitForSeconds = new WaitForSeconds(0.01f);
    
    [Inject]
    public void Construct(IStatsService statsService)
    {
        m_StatsService = statsService;
    }
    
     private void Start()
    {
        GenerateSkinViews();
    }

    protected override void OnGamePhaseChanged(GamePhase _GamePhase)
    {
        base.OnGamePhaseChanged(_GamePhase);

        switch (_GamePhase)
        {
            case GamePhase.SKIN_SHOP:
                SceneManager.LoadSceneAsync("SkinRenderer", LoadSceneMode.Additive).completed += operation =>
                {
                    RefreshSelectedSkin();
                    StartCoroutine(SetSkinPanelsVisibility(true));
                    Transition(true);
                };
                break;
            
            case GamePhase.MAIN_MENU:
                if (!m_Visible) return;
                SceneManager.UnloadSceneAsync("SkinRenderer");
                Transition(false);
                StartCoroutine(SetSkinPanelsVisibility(false));
                break;
        }
    }
    
    private void RefreshSelectedSkin()
    {
        int selectedSkinIndex = m_StatsService.FavoriteSkin;
        m_SelectedBrushRenderer.uvRect = SkinGridTextureRenderer.GetGridUvRect(selectedSkinIndex);
        GameService.SetColor(GameService.ComputeCurrentPlayerColor(true, 0));
        
        foreach (var skinViewPanel in m_SkinViewPanels)
        {
            skinViewPanel.RefreshSelectedIndicator(selectedSkinIndex);
        }
    }

    public void SelectSkin(int skinIndex)
    {
        m_StatsService.FavoriteSkin = skinIndex;
        RefreshSelectedSkin();
    }

    public void SetTitleColor(Color color)
    {
        foreach (var imageToColor in m_ColoredImages)
        {
            imageToColor.color = color;
        }
    }

    private void GenerateSkinViews()
    {
        m_SkinViewPanels = new SkinViewPanel[GameService.m_Skins.Count];
        for (int i = 0; i < GameService.m_Skins.Count; i++)
        {
            var skinViewPanel = Instantiate(m_SkinViewPanelPrefab, m_SkinViewsContainer);
            skinViewPanel.SetPanelData(i);
            skinViewPanel.gameObject.SetActive(false);
            m_ColoredImages.Add(skinViewPanel.BackgroundImage);
            m_SkinViewPanels[i] = skinViewPanel;
        }
    }

    private IEnumerator SetSkinPanelsVisibility(bool visible)
    {
        foreach (var skinViewPanel in m_SkinViewPanels)
        {
            skinViewPanel.gameObject.SetActive(visible);
            if (visible)
            {
                yield return m_AnimWaitForSeconds;
            }
        }
    }


    public void CloseShopView()
    {
        GameService.ChangePhase(GamePhase.MAIN_MENU);
    }
}
