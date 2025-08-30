using System;
using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{

        public class PlayerResetManaAction : PlayerAction
        {
            public PlayerResetManaAction(Player player, Action requiredAction = null) : base(player, requiredAction)
            {
            }

            protected override bool Perform()
            {
                player.ResetMana();
                return true;
            }

            public override ActionState ToActionState()
            {
                var actionState = new PlayerResetManaActionState();
                actionState.playerNum = player.playerNum;
                actionState.newManaLeft = player.manaLeft;
                return actionState;
            }
        }
    }
}
