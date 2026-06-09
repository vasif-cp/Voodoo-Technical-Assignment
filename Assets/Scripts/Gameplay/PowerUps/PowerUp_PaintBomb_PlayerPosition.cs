using System.Collections;
using UnityEngine;
using Zenject;

public class PowerUp_PaintBomb_PlayerPosition : PowerUp_PaintBomb_Base
{
    protected override Vector3 GetAffectPosition(Player player, float radius)
    {
        return player.transform.position;
    }
}
