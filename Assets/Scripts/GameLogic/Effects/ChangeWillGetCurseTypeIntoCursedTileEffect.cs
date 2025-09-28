using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic{

    using GameAction;

    namespace GameEffect{
        public class ChangeWillGetCurseTypeIntoCursedTileEffect : TileEffect, CanBeActivatedInterface, AffectsTilesInterface
        {

            public ChangeWillGetCurseTypeIntoCursedTileEffect(Tile tile) : base(tile){
            }

            public void Activate()
            {
                Game.currentGame.PileAction(new TileChangeTypeAction(associatedTile, TileType.Cursed));
            }

            public bool CanBeActivated()
            {
                return associatedTile != Tile.noTile && associatedTile.tileType == TileType.WillGetCursed;
            }

            public bool CheckTriggerToActivate(Action action)
            {
                switch(action){
                    case PlayerEndTurnAction playerEndTurnAction:
                        return playerEndTurnAction.wasPerformed;

                    default : return false;
                }
            }
        }
    }
}
