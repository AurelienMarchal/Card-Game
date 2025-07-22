using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{
        public class EntityPayHeartCostAction : EntityCostAction
        {

            public HeartType[] heartCost
            {
                get;
                private set;
            }

            public EntityPayHeartCostAction(Entity entity, HeartType[] heartCost, Action requiredAction = null) : base(entity, requiredAction)
            {
                this.heartCost = heartCost;
            }

            protected override bool Perform()
            {
                return entity.TryToPayHeartCost(heartCost);
            }
            
            public override ActionState ToActionState()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
