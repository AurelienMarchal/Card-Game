using System.Collections;
using System.Collections.Generic;
using GameLogic.GameBuff;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{
        public class EntityGainsBuffsAction : EntityAction{
            public List<Buff> buffs{
                get;
                private set;
            }

            public EntityGainsBuffsAction(Entity entity, List<Buff> buffs, Action requiredAction = null) : base(entity, requiredAction)
            {  
                this.buffs = buffs;
            }

            protected override bool Perform()
            {
                return false;
            }

            public override ActionState ToActionState()
            {
                var actionState = new EntityActionState();
                actionState.entityNum = entity.num;
                actionState.playerNum = entity.player.playerNum;
                return actionState;
            }
        }
    }
}
