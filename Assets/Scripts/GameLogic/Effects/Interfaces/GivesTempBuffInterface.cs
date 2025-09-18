using System.Collections.Generic;



namespace GameLogic
{

    using GameBuff;
    namespace GameEffect
    {
        public interface GivesTempBuffInterface
        {

            public void UpdateTempBuffs();

            public List<Buff> GetTempBuffs();
        }
    }
}
