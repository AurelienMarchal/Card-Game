using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;


namespace GameLogic{

    namespace GameAction{
        public class EntityResetMovementAction : EntityAction
        {
            public EntityResetMovementAction(Entity entity, Action requiredAction = null) : base(entity, requiredAction)
            {

            }

            protected override bool Perform()
            {
                entity.ResetMovement();
                return true;
            }
            
            public override ActionState ToActionState()
            {
                var actionState = new EntityResetMovementActionState();
                actionState.entityNum = entity.num;
                actionState.playerNum = entity.player.playerNum;
                actionState.newMovementLeft = entity.movementLeft;
                return actionState;
            }
        }
    }
}
