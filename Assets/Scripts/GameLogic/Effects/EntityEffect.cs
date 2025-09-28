using System.Collections;
using System.Collections.Generic;
using GameLogic.GameAction;
using UnityEngine;

namespace GameLogic{

    namespace GameEffect{
        public abstract class EntityEffect : Effect, AffectsEntitiesInterface{
            
            public Entity associatedEntity{
                get;
                protected set;
            }

            public EntityEffect(Entity entity, bool displayOnUI = true) : base(displayOnUI: displayOnUI)
            {
                associatedEntity = entity;
            }

            public virtual bool CheckTriggerToUpdateEntitiesAffected(Action action)
            {
                return false;
            }

            public virtual void UpdateEntitiesAffected()
            {
                
            }

            public virtual List<Entity> GetEntitiesAffected()
            {
                return new List<Entity> { associatedEntity };
            }
        }
    }
}
