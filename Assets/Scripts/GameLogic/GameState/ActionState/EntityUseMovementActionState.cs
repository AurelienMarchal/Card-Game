namespace GameLogic
{
    namespace GameState
    {
        public class EntityUseMovementActionState : EntityActionState
        {
            public int movementUsed
            {
                get;
                set;
            }
            
            public int newMovementLeft
            {
                get;
                set;
            }
        }
    }
}