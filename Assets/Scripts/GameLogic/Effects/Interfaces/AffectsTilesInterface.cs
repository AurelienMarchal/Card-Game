using System.Collections.Generic;


namespace GameLogic
{
    namespace GameEffect
    {
        public interface AffectsTilesInterface
        {
            public void UpdateTilesAffected();
            public List<Tile> GetTilesAffected();
        }
    }
}
