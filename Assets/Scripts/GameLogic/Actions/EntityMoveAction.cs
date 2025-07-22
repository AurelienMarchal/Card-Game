using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{
        public class EntityMoveAction : EntityAction{
            
            public Tile startTile{
                get;
                protected set;
            }

            public Tile endTile{
                get;
                protected set;
            }

            public EntityMoveAction(Entity entity, Tile startTile, Tile endTile,  Action requiredAction = null) : base(entity, requiredAction)
            {
                this.startTile = startTile;
                this.endTile = endTile;
            }

            protected override bool Perform(){
                return entity.TryToMove(endTile);
            }

            public override ActionState ToActionState()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
