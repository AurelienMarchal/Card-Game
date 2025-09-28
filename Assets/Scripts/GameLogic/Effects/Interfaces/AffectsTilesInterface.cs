using System.Collections.Generic;


namespace GameLogic
{
    namespace GameEffect
    {
        using GameAction;
        public interface AffectsTilesInterface
        {
            public bool CheckTriggerToUpdateTilesAffected(Action action);
            public void UpdateTilesAffected();
            public List<Tile> GetTilesAffected();
        }
    }
}
