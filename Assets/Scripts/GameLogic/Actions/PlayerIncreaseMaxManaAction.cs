using System;
using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{

        public class PlayerIncreaseMaxManaAction : PlayerAction
        {
            public PlayerIncreaseMaxManaAction(Player player, Action requiredAction = null) : base(player, requiredAction)
            {
            }



            protected override bool Perform()
            {
                return player.TryToIncreaseMaxMana();
            }

            public override ActionState ToActionState()
            {
                var actionState = new PlayerIncreaseMaxManaActionState();
                actionState.playerNum = player.playerNum;
                actionState.newMaxMana = player.maxMana;
                return actionState;
            }
        }
    }
}
