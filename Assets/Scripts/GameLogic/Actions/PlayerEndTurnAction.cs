using GameLogic.GameState;

namespace GameLogic{

    namespace GameAction{
        public class PlayerEndTurnAction : PlayerAction
        {
            public PlayerEndTurnAction(Player player, Action requiredAction = null) : base(player, requiredAction)
            {
            }

            protected override bool Perform()
            {
                var changeTurn = Game.currentGame.EndPlayerTurn();
                if (changeTurn)
                {
                    Game.currentGame.PileAction(new StartTurnAction(this));
                }
                else
                {
                    Game.currentGame.PileAction(new PlayerStartTurnAction(Game.currentGame.currentPlayer, this));
                }

                return true;
            }
            
            public override ActionState ToActionState()
            {
                return new PlayerEndTurnActionState(player.playerNum);
            }
        }
    }
}