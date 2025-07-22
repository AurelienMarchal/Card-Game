using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{
        public class PlayerStartTurnAction : PlayerAction
        {
            public PlayerStartTurnAction(Player player, Action requiredAction = null) : base(player, requiredAction)
            {

            }

            protected override bool Perform()
            {
                foreach (var entity in player.entities)
                {
                    Game.currentGame.PileAction(new EntityResetMovementAction(entity, this));
                }

                //Game.currentGame.PileAction(new PlayerResetManaAction(player, this));
                //Game.currentGame.PileAction(new PlayerIncreaseMaxManaAction(player, this));
                return true;
            }

            public override ActionState ToActionState()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
