using System.Collections.Generic;


namespace GameLogic{

    using GameAction;

    namespace GameEffect{
        public class ChangeTileEntityEffect : EntityEffect, CanBeActivatedInterface, AffectsTilesInterface{

            public TileType tileType{
                get;
                protected set;
            }

            public Tile tile;

            public ChangeTileEntityEffect(TileType tileType, Entity entity) : base(entity) {
                this.tileType = tileType;
                tile = associatedEntity.currentTile;
            }

            public void Activate()
            {
                Game.currentGame.PileAction(new TileChangeTypeAction(associatedEntity.currentTile, tileType));
            }

            public bool CanBeActivated()
            {
                return associatedEntity != Entity.noEntity && tile != Tile.noTile && tile.tileType != tileType;
            }

            public override string GetEffectName()
            {
                return "Change tile type under entity";
            }

            public override string GetEffectText()
            {
                return $"Change tile under {associatedEntity} to {tileType.ToTileString()}";
            }

            public bool CheckTriggerToActivate(Action action)
            {
                switch (action)
                {
                    case EntityMoveAction entityMoveAction:
                        return entityMoveAction.wasPerformed && entityMoveAction.entity == associatedEntity;
                }
                return false;
            }

            public System.Type[] ActionTypeTriggersToActivate()
            {
                return new System.Type[1]{typeof(EntityMoveAction)};
            }

            public bool CheckTriggerToUpdateTilesAffected(Action action)
            {
                switch (action)
                {
                    case PlayerSpawnEntityAction playerSpawnEntityAction:
                        return playerSpawnEntityAction.wasPerformed && playerSpawnEntityAction.entity == associatedEntity;
                    case EntityMoveAction entityMoveAction:
                        return entityMoveAction.wasPerformed && entityMoveAction.entity == associatedEntity;
                    case EntityDieAction entityDieAction:
                        return entityDieAction.wasPerformed && entityDieAction.entity == associatedEntity;

                }

                return false;
            }

            public System.Type[] ActionTypeTriggersToUpdateTilesAffected()
            {
                return new System.Type[3]{typeof(PlayerSpawnEntityAction), typeof(EntityMoveAction), typeof(EntityDieAction)};
            }

            public void UpdateTilesAffected()
            {
                tile = associatedEntity.currentTile;
            }

            public List<Tile> GetTilesAffected()
            {
                return new List<Tile> { tile };
            }

            

            
        }
    }
}
