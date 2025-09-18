


namespace GameLogic
{
    using GameAction;
    namespace GameEffect
    {
        public interface CanBeTriggeredInterface
        {

            public bool CheckTrigger(Action action);

            protected void Trigger(Action action);

            public bool TryToTrigger(Action action)
            {
                var result = CheckTrigger(action);
                if (result)
                {
                    Trigger(action);
                }
                return result;
            }

        }
    }
}
