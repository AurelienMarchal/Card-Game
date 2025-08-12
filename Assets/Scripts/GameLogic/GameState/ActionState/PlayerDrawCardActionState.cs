namespace GameLogic
{
    namespace GameState
    {
        public class PlayerDrawCardActionState : PlayerActionState
        {
            public CardState card
            {
                get;
                set;
            }

            public bool cardWasAddedToHand
            {
                get;
                set;
            }
        }
    }
}