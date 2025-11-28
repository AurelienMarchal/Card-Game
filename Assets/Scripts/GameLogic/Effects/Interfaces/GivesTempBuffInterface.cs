using System.Collections.Generic;
using System;


namespace GameLogic
{

    using GameBuff;
    using GameLogic.GameAction;

    namespace GameEffect
    {
        public interface GivesTempBuffInterface
        {

            public bool CheckTriggerToUpdateTempBuffs(Action action);

            public Type[] ActionTypeTriggersToUpdateTempBuffs();
            public void UpdateTempBuffs();

            public List<Buff> GetTempBuffs();
        }
    }
}
