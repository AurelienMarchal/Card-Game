using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWillGetCurseTypeIntoCursedTileEffect : TileEffect
{

    public ChangeWillGetCurseTypeIntoCursedTileEffect(Tile tile) : base(tile){
    }

    protected override void Activate()
    {
        Game.currentGame.PileAction(new TileChangeTypeAction(associatedTile, TileType.Cursed, effectActivatedAction));
    }

    public override bool CanBeActivated()
    {
        return base.CanBeActivated() && associatedTile.tileType == TileType.WillGetCursed;
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
