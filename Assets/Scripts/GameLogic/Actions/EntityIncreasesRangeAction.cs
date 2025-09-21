using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{

        public class EntityIncreasesRangeAction : EntityAction
        {

            public int rangeIncrease
            {
                get;
                private set;
            }

            public EntityIncreasesRangeAction(Entity entity, int rangeIncrease, Action requiredAction = null) : base(entity, requiredAction)
            {
                this.rangeIncrease = rangeIncrease;
            }

            protected override bool Perform()
            {
                return entity.TryToIncreaseRange(rangeIncrease);
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