using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class PowerUp_PaintBomb_Base : PowerUp
{
    public float 			m_Radius = 6.0f;
    public float			m_FillDuration = 0.3f;
    private float           m_RadiusMultiplier = 1.0f;
    
    protected ITerrainService	m_TerrainService;
    
    [Inject]
    public void ChildConstruct(ITerrainService terrainService)
    {
        m_TerrainService = terrainService;
    }
    
    protected abstract Vector3 GetAffectPosition (Player player, float radius);
    
    public override void OnPlayerTouched (Player player)
    {
        base.OnPlayerTouched(player);

        m_RadiusMultiplier = Mathf.Clamp(player.GetSize() / player.GetMinSize(), 1f, 2.5f);
        float radius = m_Radius * m_RadiusMultiplier;

        Vector3 pos = GetAffectPosition(player, radius);
        ApplyEffect(player, pos, radius);
    }
    
    private void ApplyEffect (Player player, Vector3 position, float radius)
    {
        m_TerrainService.FillCircle(player, position, radius, m_FillDuration, SelfDestroy);
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
