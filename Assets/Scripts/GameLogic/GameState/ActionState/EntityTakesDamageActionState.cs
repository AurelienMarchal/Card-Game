namespace GameLogic
{
    namespace GameState
    {
        public class EntityTakesDamageActionState : EntityActionState
        {
            public DamageState damageState
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