using System.Collections;
using System.Collections.Generic;
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
        }
    }
}
