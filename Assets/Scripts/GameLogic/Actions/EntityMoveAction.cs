using GameLogic.GameState;


namespace GameLogic{

    namespace GameAction{
        public class EntityMoveAction : EntityAction{
            
            public Tile startTile{
                get;
                protected set;
            }

            public Tile endTile{
                get;
                protected set;
            }

            public EntityMoveAction(Entity entity, Tile startTile, Tile endTile,  Action requiredAction = null) : base(entity, requiredAction)
            {
                this.startTile = startTile;
                this.endTile = endTile;
            }

            protected override bool Perform(){
                return entity.TryToMove(endTile);
            }

            public override ActionState ToActionState()
            {
                var actionState = new EntityMoveActionState();
                actionState.entityNum = entity.num;
                actionState.playerNum = entity.player.playerNum;
                actionState.endTileNum = endTile.num;
                actionState.startTileNum = startTile.num;
                return actionState;
            }
        }
    }
}
