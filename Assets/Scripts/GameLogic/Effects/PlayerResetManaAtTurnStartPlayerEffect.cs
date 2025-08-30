using GameLogic.GameAction;

namespace GameLogic{

    namespace GameEffect{

        public class PlayerResetManaAtTurnStartPlayerEffect : PlayerEffect
        {
            public PlayerResetManaAtTurnStartPlayerEffect(Player player) : base(player)
            {
            }
            
            protected override void Activate(){
                Game.currentGame.PileAction(new PlayerResetManaAction(associatedPlayer, effectActivatedAction));
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