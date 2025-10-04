using GameLogic.GameAction;

namespace GameLogic{

    namespace GameEffect{

        public class PlayerIncreaseMaxManaAtTurnStartPlayerEffect : PlayerEffect, CanBeActivatedInterface
        {
            public PlayerIncreaseMaxManaAtTurnStartPlayerEffect(Player player) : base(player)
            {
            }
            
            void CanBeActivatedInterface.Activate(){
                Game.currentGame.PileAction(new PlayerIncreaseMaxManaAction(associatedPlayer));
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