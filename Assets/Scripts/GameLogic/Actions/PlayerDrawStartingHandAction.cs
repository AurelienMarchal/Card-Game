using GameLogic.GameState;

namespace GameLogic{

    namespace GameAction{
        //TODO
        public class PlayerDrawStartingHAndAction : PlayerAction
        {
            public PlayerDrawStartingHAndAction(Player player, Action requiredAction = null) : base(player, requiredAction)
            {
            }

            protected override bool Perform()
            {

                return true;
            }

            public override ActionState ToActionState()
            {
                var actionState = new PlayerEndTurnActionState();
                actionState.playerNum = player.playerNum;
                return actionState;
            }
        }
    }
}