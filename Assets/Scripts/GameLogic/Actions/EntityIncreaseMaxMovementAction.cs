using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{

        public class EntityIncreaseMaxMovementAction : EntityAction
        {
            public EntityIncreaseMaxMovementAction(Entity entity, Action requiredAction = null) : base(entity, requiredAction)
            {
            }

            protected override bool Perform()
            {
                return entity.TryToIncreaseMaxMovement();
            }

            public override ActionState ToActionState()
            {
                var actionState = new EntityIncreaseMaxMovementActionState();
                actionState.entityNum = entity.num;
                actionState.playerNum = entity.player.playerNum;
                return actionState;
            }
        }
    }
}
