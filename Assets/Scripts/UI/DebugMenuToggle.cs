using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugMenuToggle : MonoBehaviour
{
    [SerializeField] private Toggle   m_Toggle;
    [SerializeField] private TMP_Text m_NameText;

    private GameFeature     m_Feature;
    private IFeatureService m_FeatureService;

    public void Bind(GameFeature feature, IFeatureService featureService)
    {
        m_Feature        = feature;
        m_FeatureService = featureService;

        m_NameText.text = feature.ToString();
        m_Toggle.SetIsOnWithoutNotify(featureService.IsEnabled(feature));  
        m_Toggle.onValueChanged.AddListener(value => m_FeatureService.SetEnabled(m_Feature, value));
    }

    public void Refresh() => m_Toggle.SetIsOnWithoutNotify(m_FeatureService.IsEnabled(m_Feature));

}
