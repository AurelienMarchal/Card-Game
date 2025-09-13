using System.Collections.Generic;



namespace GameLogic
{

    using GameBuff;
    namespace GameEffect
    {
        public interface GivesTempBuff
        {
            public List<Buff> GetTempBuffs();
        }
    }
}
