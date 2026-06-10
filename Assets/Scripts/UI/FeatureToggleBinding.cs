using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FeatureToggleBinding : MonoBehaviour
{
    [SerializeField] private GameFeature m_Feature;
    [SerializeField] private GameObject[]  m_TargetsToEnable;
    [SerializeField] private GameObject[]  m_TargetsToDisable;

    private IFeatureService m_FeatureService;

    [Inject]
    public void Construct(IFeatureService featureService)
    {
        m_FeatureService = featureService;
    }

    private void OnEnable()
    {
        m_FeatureService.OnFeatureChanged += HandleChanged;
    }

    private void OnDisable()
    {
        m_FeatureService.OnFeatureChanged -= HandleChanged;
    }

    private void HandleChanged(GameFeature f, bool isEnabled)
    {
        if (f == m_Feature)
        {
            foreach (var targetGameObject in m_TargetsToEnable)
            {
                targetGameObject.SetActive(isEnabled);
            }
            
            foreach (var targetGameObject in m_TargetsToDisable)
            {
                targetGameObject.SetActive(!isEnabled);
            }
        }
    }
}

