using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTileActivableEffect : ActivableEffect
{

    public TileType tileType{
        get;
        protected set;
    }

    public ChangeTileActivableEffect(TileType tileType, Entity entity, Cost cost) : base(entity, cost){
        this.tileType = tileType;
    }

    protected override void Activate()
    {
        Game.currentGame.PileAction(new TileChangeTypeAction(associatedEntity.currentTile, tileType, effectActivatedAction), false);
    }

    public override string GetEffectText()
    {
        return $"Change tile under {associatedEntity} to {tileType.ToTileString()}";
    }
}
