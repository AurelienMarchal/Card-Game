using System;
using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{
        [Obsolete]
        public class PlayerResetManaAction : PlayerAction
        {
            public PlayerResetManaAction(Player player, Action requiredAction = null) : base(player, requiredAction)
            {
            }

            public override ActionState ToActionState()
            {
                throw new NotImplementedException();
            }

            protected override bool Perform()
            {
                player.ResetMana();
                return true;
            }
        }
    }
}
