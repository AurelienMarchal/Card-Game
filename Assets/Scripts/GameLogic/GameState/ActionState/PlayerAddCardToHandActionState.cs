namespace GameLogic
{
    namespace GameState
    {
        public class PlayerAddCardToHandActionState : PlayerActionState
        {
            public CardState card
            {
                get;
                set;
            }

            public int position
            {
                get;
                set;
            }

            public HandState newHandState{
                get;
                set;
            }

        }
    }
}