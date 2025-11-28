using System;

namespace GameLogic
{
    namespace GameEffect
    {
        using GameAction;
        public interface HasRangeInterface
        {
            public bool CheckTriggerToUpdateRange(Action action);

            public Type[] ActionTypeTriggersToUpdateRange();

            public void UpdateRange();
            public int GetRange();
        }
    }
}
