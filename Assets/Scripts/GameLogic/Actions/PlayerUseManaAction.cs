
using System;
using GameLogic.GameState;

namespace GameLogic{

    namespace GameAction{

        public class PlayerUseManaAction : PlayerAction{

            public int numberOfMana{
                get;
                protected set;
            }

            public PlayerUseManaAction(Player player, int numberOfMana, Action requiredAction = null) : base(player, requiredAction){
                this.numberOfMana = numberOfMana;
            }

            protected override bool Perform(){
                return player.TryToUseMana(numberOfMana);
            }

            public override ActionState ToActionState()
            {
                var actionState = new PlayerUseManaActionState();
                actionState.playerNum = player.playerNum;
                actionState.newManaLeft = player.manaLeft;
                actionState.manaUsed = numberOfMana;
                return actionState;
            }
        }
    }
}
