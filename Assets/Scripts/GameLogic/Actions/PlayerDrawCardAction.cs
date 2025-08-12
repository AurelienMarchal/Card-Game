using GameLogic.GameState;

namespace GameLogic{

    namespace GameAction{
        public class PlayerDrawCardAction : PlayerAction
        {

            public Card card
            {
                get;
                private set;
            }

            public bool cardWasAddedToHand
            {
                get;
                private set;
            }

            public PlayerDrawCardAction(Player player, Action requiredAction = null) : base(player, requiredAction)
            {
                card = null;
                cardWasAddedToHand = false;
            }

            protected override bool Perform()
            {
                card = player.TryToDraw();
                if (card != null)
                {
                    if (player.hand.CanAddCard(card, position: -1))
                    {
                        cardWasAddedToHand = true;
                        Game.currentGame.PileAction(new PlayerAddCardToHandAction(player, card, position: -1, requiredAction: this));
                    }
                    else
                    {
                        //Burn Card
                        cardWasAddedToHand = false;
                    }
                }
                else
                {
                    cardWasAddedToHand = false;
                }
                return card != null;
            }
            
            //TODO
            public override ActionState ToActionState()
            {
                var actionState = new PlayerDrawCardActionState();
                actionState.card = card == null ? null : card.ToCardState();
                actionState.cardWasAddedToHand = cardWasAddedToHand;
                actionState.playerNum = player.playerNum;
                return actionState;
            }
        }
    }
}