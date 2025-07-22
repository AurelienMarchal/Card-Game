using System;
using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{
        [Obsolete]
        public class PlayerIncreaseMaxManaAction : PlayerAction
        {
            public PlayerIncreaseMaxManaAction(Player player, Action requiredAction = null) : base(player, requiredAction)
            {
            }

            public override ActionState ToActionState()
            {
                throw new System.NotImplementedException();
            }

            protected override bool Perform()
            {
                return player.TryToIncreaseMaxMana();
            }
        }
    }
}
