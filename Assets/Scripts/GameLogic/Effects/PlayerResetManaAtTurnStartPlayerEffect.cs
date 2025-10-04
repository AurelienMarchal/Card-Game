using GameLogic.GameAction;

namespace GameLogic{

    namespace GameEffect{

        public class PlayerResetManaAtTurnStartPlayerEffect : PlayerEffect, CanBeActivatedInterface
        {
            public PlayerResetManaAtTurnStartPlayerEffect(Player player) : base(player)
            {
            }
            
            void CanBeActivatedInterface.Activate(){
                Game.currentGame.PileAction(new PlayerResetManaAction(associatedPlayer));
            }

            public bool CanBeActivated()
            {
                return true;
            }

            public bool CheckTriggerToActivate(Action action)
            {
                switch (action)
                {
                    case PlayerStartTurnAction playerStartTurnAction:
                        return playerStartTurnAction.wasPerformed && playerStartTurnAction.player == associatedPlayer;


                    default: return false;
                }
            }
        }
    }
}