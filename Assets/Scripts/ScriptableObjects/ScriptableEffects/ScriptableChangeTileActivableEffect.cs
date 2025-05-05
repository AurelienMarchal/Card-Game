
using UnityEngine;

using GameLogic;
using GameLogic.GameEffect;

[CreateAssetMenu(fileName = "ChangeTileActivableEffect", menuName = "Effect/ActivableEffect/Change Tile", order = 0)]
public class ScriptableChangeTileActivableEffect : ScriptableActivableEffect
{
    public TileType tileType;

    public override Effect GetEffect()
    {
        return new ChangeTileActivableEffect(tileType, Entity.noEntity, cost);
    }
}
