using UnityEngine;


namespace GameLogic{

    namespace GameEffect{

        using GameAction;
        public class MoveToChangeTileTypeEffect : EntityEffect, CanBeActivatedInterface
        {

            public TileType tileType{
                get;
                protected set;
            }

            public MoveToChangeTileTypeEffect(Entity entity, TileType tileType) : base(entity){
                associatedEntity = entity;
                this.tileType = tileType;
            }

            void CanBeActivatedInterface.Activate()
            {
                Game.currentGame.PileAction(new TileChangeTypeAction(associatedEntity.currentTile, tileType));
            }

            bool CanBeActivatedInterface.CanBeActivated()
            {
                return associatedEntity != Entity.noEntity;
            }
            

            public bool CheckTriggerToActivate(Action action)
            {
                switch(action){
                    case EntityMoveAction entityMoveAction: 
                        if(entityMoveAction.wasPerformed && entityMoveAction.entity == associatedEntity){
                            return true;
                        }
                        return false;

                    default : return false;
                }
            }
            public System.Type[] ActionTypeTriggersToActivate()
            {
                return new System.Type[1]{typeof(EntityMoveAction)};
            }

            public override string GetEffectText(){
                return $"Every time {associatedEntity} moves, the tile under it is transformed into a {tileType.ToTileString()}";
            }

            
        }
    }
}