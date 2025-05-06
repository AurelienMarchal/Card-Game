using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{
        public class PlayerResetManaAction : PlayerAction{
            public PlayerResetManaAction(Player player, Action requiredAction = null) : base(player, requiredAction)
            {
            }

            protected override bool Perform()
            {
                player.ResetMana();
                return true;
            }
        }
    }
}
