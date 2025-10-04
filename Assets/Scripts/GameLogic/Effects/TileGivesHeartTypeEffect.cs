namespace GameLogic{

    namespace GameEffect{
        using GameAction;
        using UnityEngine;

        public class TileGivesHeartTypeEffect : TileEffect, CanBeActivatedInterface{

            public TileType tileType{
                get;
                protected set;
            }

            public HeartType heartType{
                get;
                protected set;
            }

            Entity entityThatJustWalkedOnTop = Entity.noEntity;

            public TileGivesHeartTypeEffect(Tile tile, TileType tileType, HeartType heartType) : base(tile)
            {
                this.tileType = tileType;
                this.heartType = heartType;
            }

            public bool CanBeActivated(){
                return associatedTile != Tile.noTile && associatedTile.tileType == tileType && entityThatJustWalkedOnTop != Entity.noEntity;
            }

            void CanBeActivatedInterface.Activate()
            {
                Game.currentGame.PileAction(new EntityGainHeartAction(entityThatJustWalkedOnTop, heartType));
                //entityThatJustWalkedOnTop = Entity.noEntity;
            }

            public bool CheckTriggerToActivate(Action action)
            {
                switch(action){
                    case EntityMoveAction entityMoveAction:
                            var condition = entityMoveAction.wasPerformed && entityMoveAction.endTile == associatedTile;
                            if(condition){
                                entityThatJustWalkedOnTop = entityMoveAction.entity;
                            }
                            return condition;

                    default : return false;
                }
            }
        }

    }
}