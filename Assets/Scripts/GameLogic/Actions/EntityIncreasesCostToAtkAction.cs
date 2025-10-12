using System;
using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{

        public class EntityIncreasesCostToAtkAction : EntityAction
        {

            public int mouvementCostIncrease
            {
                get;
                private set;
            }

            public int manaCostIncrease
            {
                get;
                private set;
            }

            public HeartType[] heartCostIncrease
            {
                get;
                private set;
            }

            public EntityIncreasesCostToAtkAction(Entity entity, int mouvementCostIncrease, int manaCostIncrease, HeartType[] heartCostIncrease, Action requiredAction = null) : base(entity, requiredAction)
            {
                this.mouvementCostIncrease = mouvementCostIncrease;
                this.manaCostIncrease = manaCostIncrease;
                this.heartCostIncrease = heartCostIncrease;
            }

            protected override bool Perform()
            {
                return entity.TryToIncreaseCostToAtk(mouvementCostIncrease, manaCostIncrease, heartCostIncrease);
            }

            public override ActionState ToActionState()
            {
                var actionState = new EntityIncreasesCostToAtkActionState();
                actionState.entityNum = entity.num;
                actionState.playerNum = entity.player.playerNum;
                actionState.newCost = entity.costToMove.ToCostState();
                
                return actionState;
            }
        }
    }
}