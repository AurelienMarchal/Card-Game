

namespace GameLogic
{
    using GameAction;
    namespace GameEffect
    {
        public interface DealsDamageInterface
        {
            public bool CheckTriggerToUpdateDamage(Action action);

            public void UpdateDamage();
            public Damage GetDamage();
        }
    }
}
