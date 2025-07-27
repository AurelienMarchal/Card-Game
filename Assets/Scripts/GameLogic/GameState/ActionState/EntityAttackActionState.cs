namespace GameLogic
{
    namespace GameState
    {
        public class EntityAttackActionState : EntityActionState
        {
            public uint attackedEntityPlayerNum { get; set; }
            public uint attackedEntityNum { get; set; }
            
            public bool isCounterAttack { get; set; }
            

            public EntityAttackActionState()
            {

            }
        }
    }
}