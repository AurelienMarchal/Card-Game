

namespace GameLogic
{
    namespace GameEffect
    {
        using GameAction;
        public interface HasCostInterface
        {
            public bool CheckTriggerToUpdateCost(Action action);

            public void UpdateCost();
            public Cost GetCost();
        }
    }
}
