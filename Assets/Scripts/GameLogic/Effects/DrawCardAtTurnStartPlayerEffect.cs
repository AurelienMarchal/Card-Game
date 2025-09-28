using GameLogic.GameAction;

namespace GameLogic{

    namespace GameEffect{

        public class DrawCardAtTurnStartPlayerEffect : PlayerEffect, CanBeActivatedInterface
        {
            public DrawCardAtTurnStartPlayerEffect(Player player) : base(player)
            {
            }

            public override string GetEffectName()
            {
                return "Draw card at the start of the turn";
            }

            public override string GetEffectText()
            {
                return GetEffectName();
            }
            
            public void Activate()
            {
                Game.currentGame.PileAction(new PlayerDrawCardAction(associatedPlayer));
            }

            public bool CanBeActivated()
            {
                return associatedPlayer.CanDraw();
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