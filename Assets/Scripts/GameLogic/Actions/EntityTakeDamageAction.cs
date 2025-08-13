using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;


namespace GameLogic{

    namespace GameAction{
        public class EntityTakeDamageAction : EntityAction{   

            public Damage damage{
                get;
                protected set;
            }

            //Damage type
            public EntityTakeDamageAction(Entity entity, Damage damage,  Action requiredAction = null) : base(entity, requiredAction){
                this.damage = damage;
            }

            protected override bool Perform(){
                entity.TakeDamage(damage);
                return true;
            }

            public override ActionState ToActionState()
            {
                var actionState = new EntityTakesDamageActionState();
                actionState.entityNum = entity.num;
                actionState.playerNum = entity.player.playerNum;
                actionState.damageState = damage.ToDamageState();
                actionState.newHealthState = entity.health.ToHealthState();
                return actionState;
            }
        }
    }
}
