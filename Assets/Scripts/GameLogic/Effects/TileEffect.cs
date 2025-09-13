
namespace GameLogic{

    namespace GameEffect{
        public class TileEffect : Effect{
            public Tile associatedTile{
                get;
                protected set;
            }


            public TileEffect(Tile tile, bool displayOnUI = true) : base(displayOnUI:displayOnUI){
                associatedTile = tile;
            }

            public override bool CanBeActivated()
            {
                return associatedTile != Tile.noTile;
            }
        }
    }
}
