using System;

namespace GameLogic
{
    
    using GameAction;
    namespace GameEffect
    {
        public interface DealsDamageInterface
        {
            public bool CheckTriggerToUpdateDamage(Action action);

            public Type[] ActionTypeTriggersToUpdateDamage();

            public void UpdateDamage();
            public Damage GetDamage();
        }
    }
}
