using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{

        public class EntityIncreasesAtkDamageAction : EntityAction
        {

            public int atkIncrease
            {
                get;
                private set;
            }

            public EntityIncreasesAtkDamageAction(Entity entity, int atkIncrease, Action requiredAction = null) : base(entity, requiredAction)
            {
                this.atkIncrease = atkIncrease;
            }

            protected override bool Perform()
            {
                return entity.TryToIncreaseAtkDamage(atkIncrease);
            }

            public override ActionState ToActionState()
            {
                var actionState = new EntityIncreasesAtkDamageActionState();
                actionState.entityNum = entity.num;
                actionState.playerNum = entity.player.playerNum;
                actionState.newAtkDamage = entity.atkDamage.ToDamageState();
                
                return actionState;
            }
        }
    }
}
