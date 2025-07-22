
namespace GameLogic{

    namespace GameAction{
        public abstract class TileAction : Action{
            
            public Tile tile;

            public TileAction(Tile tile, Action requiredAction = null) : base(requiredAction){
                this.tile = tile;
            }
        }
    }
}