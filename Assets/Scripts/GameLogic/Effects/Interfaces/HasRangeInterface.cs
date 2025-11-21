

namespace GameLogic
{
    namespace GameEffect
    {
        using GameAction;
        public interface HasRangeInterface
        {
            public bool CheckTriggerToUpdateRange(Action action);

            public void UpdateRange();
            public int GetRange();
        }
    }
}
