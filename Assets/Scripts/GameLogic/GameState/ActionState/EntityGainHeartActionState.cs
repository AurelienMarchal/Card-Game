namespace GameLogic
{
    namespace GameState
    {
        public class EntityGainHeartActionState : EntityActionState
        {
            public HeartType heartType
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