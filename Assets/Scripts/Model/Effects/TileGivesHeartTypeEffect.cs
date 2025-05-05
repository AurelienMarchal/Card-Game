namespace GameLogic{

    namespace GameEffect{

        using GameAction;
        

        public class TileGivesHeartTypeEffect : TileEffect{

            public Entity affectedEntity{
                get;
                protected set;
            }

            public TileType tileType{
                get;
                protected set;
            }

            public HeartType heartType{
                get;
                protected set;
            }

            public TileGivesHeartTypeEffect(Tile tile, TileType tileType, HeartType heartType) : base(tile){
                this.tileType = tileType;
                this.heartType = heartType;
                affectedEntity = Entity.noEntity;
            }

            public override bool CanBeActivated(){
                return base.CanBeActivated() && associatedTile.tileType == tileType && affectedEntity != Entity.noEntity;
            }

            protected override void Activate()
            {   

                Game.currentGame.PileAction(new EntityGainHeartAction(affectedEntity, heartType, effectActivatedAction));
                affectedEntity = Entity.noEntity;
            }

            public override bool Trigger(Action action)
            {
                switch(action){
                    case EntityMoveAction entityMoveAction:
                            var condition = entityMoveAction.wasPerformed && entityMoveAction.endTile == associatedTile;
                            if(condition){
                                affectedEntity = entityMoveAction.entity;
                            }
                            return condition;

                    default : return false;
                }
            }
        }

    }
}