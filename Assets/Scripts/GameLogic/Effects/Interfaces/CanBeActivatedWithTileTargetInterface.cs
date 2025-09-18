
using System.Collections.Generic;


namespace GameLogic
{
    namespace GameEffect{
        public interface CanBeActivatedWithTileTargetInterface
        {
            public List<Tile> PossibleTileTargets();

            public bool CanBeActivatedWithTileTarget(Tile tile);

            protected void ActivateWithTileTarget(Tile tile);

            public bool TryToActivateWithTileTarget(Tile tile){
                var result = CanBeActivatedWithTileTarget(tile);
                if(result){
                    ActivateWithTileTarget(tile);
                }
                return result;
            }
        }
    }
}
