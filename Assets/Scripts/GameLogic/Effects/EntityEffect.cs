using System.Collections;
using System.Collections.Generic;
using GameLogic.GameAction;
using UnityEngine;

namespace GameLogic{

    using GameState;

    namespace GameEffect{
        public abstract class EntityEffect : Effect, AffectsEntitiesInterface{
            
            public Entity associatedEntity{
                get;
                protected set;
            }

            public EntityEffect(Entity entity, bool displayOnUI = true) : base(displayOnUI: displayOnUI)
            {
                associatedEntity = entity;
                metaData["associatedEntityNum"] = entity.num;
                metaData["associatedEntityPlayerNum"] = entity.player.playerNum;
            }
            

            public EntityEffect(EffectState effectState) : base (effectState)
            {
                
            }

            public virtual bool CheckTriggerToUpdateEntitiesAffected(Action action)
            {
                return false;
            }
            
            public virtual System.Type[] ActionTypeTriggersToUpdateEntitiesAffected()
            {
                return null;
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
