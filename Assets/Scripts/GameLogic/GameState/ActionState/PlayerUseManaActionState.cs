namespace GameLogic
{
    namespace GameState
    {
        public class PlayerUseManaActionState : PlayerActionState
        {
            public int manaUsed
            {
                get;
                set;
            }
            
            public int newManaLeft
            {
                get;
                set;
            }
        }
    }
}