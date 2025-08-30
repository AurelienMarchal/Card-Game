using GameLogic.GameAction;

namespace GameLogic{

    namespace GameEffect{

        public class PlayerIncreaseMaxManaAtTurnStartPlayerEffect : PlayerEffect
        {
            public PlayerIncreaseMaxManaAtTurnStartPlayerEffect(Player player) : base(player)
            {
            }
            
            protected override void Activate(){
                Game.currentGame.PileAction(new PlayerIncreaseMaxManaAction(associatedPlayer, effectActivatedAction));
            }

            public override bool CanBeActivated()
            {
                return true;
            }

            public override bool Trigger(Action action)
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