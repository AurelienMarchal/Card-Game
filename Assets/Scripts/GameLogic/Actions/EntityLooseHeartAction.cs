using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{
        public class EntityLooseHeartAction : EntityAction{
            public HeartType heartType{
                get;
                private set;
            }

            public EntityLooseHeartAction(Entity entity, HeartType heartType, Action requiredAction = null) : base(entity, requiredAction)
            {  
                this.heartType = heartType;
            }

            protected override bool Perform()
            {
                throw new System.NotImplementedException();
            }

            public override ActionState ToActionState()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
