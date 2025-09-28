using System.Collections.Generic;



namespace GameLogic
{

    using GameBuff;
    using GameLogic.GameAction;

    namespace GameEffect
    {
        public interface GivesTempBuffInterface
        {

            public bool CheckTriggerToUpdateTempBuffs(Action action);
            public void UpdateTempBuffs();

            public List<Buff> GetTempBuffs();
        }
    }
}
