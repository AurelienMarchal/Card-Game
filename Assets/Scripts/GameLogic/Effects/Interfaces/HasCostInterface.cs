using System;

namespace GameLogic
{
    namespace GameEffect
    {
        using GameAction;
        public interface HasCostInterface
        {
            public bool CheckTriggerToUpdateCost(Action action);

            public Type[] ActionTypeTriggersToUpdateCost();

            public void UpdateCost();
            public Cost GetCost();
        }
    }
}
