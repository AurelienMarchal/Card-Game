using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic{

    namespace GameEffect{
        public class EntityEffect : Effect{
            
            public Entity associatedEntity{
                get;
                protected set;
            }

            public EntityEffect(Entity entity, bool displayOnUI = true) : base(displayOnUI: displayOnUI)
            {
                associatedEntity = entity;
            }

            public void InitializeAssociatedEntity(Entity entity)
            {
                if (associatedEntity == Entity.noEntity)
                {
                    associatedEntity = entity;
                }
            }

            public override bool CanBeActivated()
            {
                return associatedEntity != Entity.noEntity;
            }
        }
    }
}
