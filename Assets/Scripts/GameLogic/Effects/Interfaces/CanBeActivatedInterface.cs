using System;

namespace GameLogic
{
    namespace GameEffect
    {

        using GameAction;
        public interface CanBeActivatedInterface
        {

            public bool CheckTriggerToActivate(Action action);

            public Type[] ActionTypeTriggersToActivate();
            public bool CanBeActivated();

            public void Activate();

            public bool TryToActivate()
            {
                var result = CanBeActivated();
                if (result)
                {
                    Activate();
                }
                return result;
            }

        }
    }
}