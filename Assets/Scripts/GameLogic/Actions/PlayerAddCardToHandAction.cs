using GameLogic.GameState;

namespace GameLogic{

    namespace GameAction{
        public class PlayerAddCardToHandAction : PlayerAction
        {

            public Card card
            {
                get;
                private set;
            }

            public int position
            {
                get;
                private set;
            }

            public PlayerAddCardToHandAction(Player player, Card card, int position, Action requiredAction = null) : base(player, requiredAction)
            {
                this.card = card;
                this.position = position;
            }

            protected override bool Perform()
            {
                return player.hand.TryToAddCard(card, position);
            }
            
            //TODO
            public override ActionState ToActionState()
            {
                var actionState = new PlayerAddCardToHandActionState();
                actionState.card = card == null ? null : card.ToCardState();
                actionState.position = position;
                actionState.playerNum = player.playerNum;
                return actionState;
            }
        }
    }
}