using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_PaintBomb_RandomPosition : PowerUp_PaintBomb_Base
{
    protected override Vector3 GetAffectPosition (Player player, float radius)
    {
        return m_TerrainService.GetRandomSurfacePosition(radius);
    }
}
