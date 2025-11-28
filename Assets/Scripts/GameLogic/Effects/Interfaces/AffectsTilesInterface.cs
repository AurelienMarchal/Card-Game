using System.Collections.Generic;
using System;

namespace GameLogic
{
    namespace GameEffect
    {
        
        using GameAction;
        public interface AffectsTilesInterface
        {
            public bool CheckTriggerToUpdateTilesAffected(Action action);

            public Type[] ActionTypeTriggersToUpdateTilesAffected();
            public void UpdateTilesAffected();
            public List<Tile> GetTilesAffected();
        }
    }
}
