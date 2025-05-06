using System.Collections;
using System.Collections.Generic;
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
        }
    }
}
