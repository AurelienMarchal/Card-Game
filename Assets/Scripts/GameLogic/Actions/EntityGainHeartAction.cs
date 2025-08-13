using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{
        public class EntityGainHeartAction : EntityAction{
            public HeartType heartType{
                get;
                private set;
            }

            public EntityGainHeartAction(Entity entity, HeartType heartType, Action requiredAction = null) : base(entity, requiredAction)
            {  
                this.heartType = heartType;
            }

            protected override bool Perform()
            {
                return entity.health.TryToGainHeart(heartType);
            }

            public override ActionState ToActionState()
            {
                var actionState = new EntityGainHeartActionState();
                actionState.entityNum = entity.num;
                actionState.playerNum = entity.player.playerNum;
                actionState.heartType = heartType;
                actionState.newHealthState = entity.health.ToHealthState();
                return actionState;
            }
        }
    }
}