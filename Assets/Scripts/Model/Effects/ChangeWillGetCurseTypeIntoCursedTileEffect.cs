using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWillGetCurseTypeIntoCursedTileEffect : TileEffect
{

    public Tile tile{
        get;
        protected set;
    }

    public ChangeWillGetCurseTypeIntoCursedTileEffect(Tile tile) : base(tile){
        this.tile = tile;
    }

    protected override void Activate()
    {
        Game.currentGame.PileAction(new TileChangeTypeAction(tile, TileType.Cursed, effectActivatedAction), false);
    }

    public override bool CanBeActivated()
    {
        return base.CanBeActivated() && tile.tileType == TileType.WillGetCursed;
    }


    public override bool Trigger(Action action)
    {
        switch(action){
            case EndPlayerTurnAction endPlayerTurnAction:
                return endPlayerTurnAction.wasPerformed;

            default : return false;
        }
    }
}
