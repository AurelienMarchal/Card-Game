using System.Collections;
using System.Collections.Generic;
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
        }
    }
}
