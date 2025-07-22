using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic{

    using GameAction;

    namespace GameEffect{
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
                    case PlayerEndTurnAction playerEndTurnAction:
                        return playerEndTurnAction.wasPerformed;

                    default : return false;
                }
            }
        }
    }
}
