using System.Collections.Generic;



namespace GameLogic
{

    using GameBuff;
    using GameLogic.GameAction;

    namespace GameEffect
    {
        public interface GivesTempEntityBuffInterface
        {

            public bool CheckTriggerToUpdateTempEntityBuffs(Action action);
            public void UpdateTempEntityBuffs();

            public List<EntityBuff> GetTempEntityBuffs();
        }
    }
}
