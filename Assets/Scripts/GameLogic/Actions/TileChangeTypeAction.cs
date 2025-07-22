using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{
        public class TileChangeTypeAction : TileAction
        {
            public TileType newType{
                get;
                protected set;
            }

            public TileChangeTypeAction(Tile tile, TileType tileType, Action requiredAction = null) : base(tile, requiredAction){
                newType = tileType;
            }

            protected override bool Perform(){
                //Check si c'est possible avant blablabla
                tile.tileType = newType;
                return true;
            }

            public override ActionState ToActionState()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
