namespace GameLogic
{
    namespace GameState
    {
        public class EntityHealsActionState : EntityActionState
        {
            public int numberOfHeartsHealed
            {
                get;
                set;
            }
            
            public HealthState newHealthState{
                get;
                set;
            }
        }
    }
}