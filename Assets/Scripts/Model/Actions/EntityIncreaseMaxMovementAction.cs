using System.Collections;
using System.Collections.Generic;
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
        }
    }
}
