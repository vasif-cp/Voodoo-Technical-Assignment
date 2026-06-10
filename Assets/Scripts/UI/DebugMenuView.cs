using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DebugMenuView : View<DebugMenuView>
{
    [SerializeField] private GameObject              m_Panel;
    [SerializeField] private RectTransform           m_Container;  
    [SerializeField] private DebugMenuToggle         m_DebugMenuTogglePrefab;

    private IFeatureService m_FeatureService;
    
    private bool m_Built;
    private readonly List<DebugMenuToggle> m_Rows = new();

    [Inject]
    public void Construct(IFeatureService featureService)
    {
        m_FeatureService = featureService;
    }

    protected override void OnGamePhaseChanged(GamePhase _GamePhase)
    {
        base.OnGamePhaseChanged(_GamePhase);
        bool show = _GamePhase == GamePhase.MAIN_MENU;  
        Transition(show);   
        
    }

    public void OnOpen()
    {
        if (!m_Built) Build();
        else foreach (var row in m_Rows) row.Refresh();  
        
        m_Panel.SetActive(true);
    }

    public void Close()
    {
        m_Panel.SetActive(false);
    }
  
    private void Build()
    {
        foreach (GameFeature feature in Enum.GetValues(typeof(GameFeature)))
        {
            var row = Instantiate(m_DebugMenuTogglePrefab, m_Container);
            row.Bind(feature, m_FeatureService);
            m_Rows.Add(row);
        }
        m_Built = true;
    }

}
