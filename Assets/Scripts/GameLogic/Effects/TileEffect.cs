


namespace GameLogic{
    using System.Collections.Generic;
    using GameAction;

    namespace GameEffect
    {
        public abstract class TileEffect : Effect, AffectsTilesInterface
        {
            public Tile associatedTile
            {
                get;
                protected set;
            }


            public TileEffect(Tile tile, bool displayOnUI = true) : base(displayOnUI: displayOnUI)
            {
                associatedTile = tile;
            }

            public virtual bool CheckTriggerToUpdateTilesAffected(Action action)
            {
                return false;
            }

            public virtual List<Tile> GetTilesAffected()
            {
                return new List<Tile> { associatedTile };
            }

            public virtual void UpdateTilesAffected()
            {

            }
        }
    }
}
