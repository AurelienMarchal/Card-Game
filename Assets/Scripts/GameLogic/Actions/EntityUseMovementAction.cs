using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;


namespace GameLogic{

    namespace GameAction{
        public class EntityUseMovementAction : EntityCostAction{
            public int movementUsed{
                get;
                private set;
            }

            public EntityUseMovementAction(int movement, Entity entity, Action requiredAction = null) : base(entity, requiredAction){
                movementUsed = movement;
            }

            protected override bool Perform()
            {
                return entity.TryToUseMovement(movementUsed);
            }

            public override ActionState ToActionState()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
