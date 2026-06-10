using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkinViewPanel : MonoBehaviour
{
    [SerializeField] private Image m_BackgroundImage;
    [SerializeField] private RawImage m_SkinRendererImage;
    [SerializeField] private GameObject m_SelectedIndicator;

    private int m_SkinIndex;

    private const float c_PunchAnimForce = 0.1f;
    private const float c_PunchAnimDuration = 0.4f;
    
    public Image BackgroundImage => m_BackgroundImage;

    private void OnEnable()
    {
        if (DOTween.IsTweening(transform)) return;
        transform.DOPunchScale(Vector3.one * c_PunchAnimForce,c_PunchAnimDuration);
    }

    public void SelectSkin()
    {
        int skinIndex = GetSkinIndex();
        SkinShopView.Instance.SelectSkin(skinIndex);
    }

    public void SetPanelData(int skinIndex)
    {
        m_SkinIndex = skinIndex;
        m_SkinRendererImage.uvRect = SkinGridTextureRenderer.GetGridUvRect(m_SkinIndex);
    }

    public void RefreshSelectedIndicator(int selectedSkinIndex)
    {
        m_SelectedIndicator.SetActive(selectedSkinIndex == GetSkinIndex());
    }

    private int GetSkinIndex()
    {
        return m_SkinIndex;
    }
}
