using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameLogic{

    namespace GameEffect{

        using GameAction;
        public class EntitySpawnEntityActivableEffect : ActivableEffect{
            public ScriptableEntity scriptableEntity{
                get;
                private set;
            }

            public EntitySpawnEntityActivableEffect(Entity entity, Cost cost, ScriptableEntity scriptableEntity) : base(entity, cost){
                this.scriptableEntity = scriptableEntity;
            }

            protected override void Activate(){

                var startingTile = Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, associatedEntity.direction);
                Game.currentGame.PileAction(new PlayerSpawnEntityAction(associatedEntity.player, scriptableEntity, startingTile, associatedEntity.direction));
            }

            public override bool CanBeActivated()
            {
                if(associatedEntity == Entity.noEntity){
                    return false;
                }

                var startingTile = Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, associatedEntity.direction);
                
                return base.CanBeActivated() && startingTile != Tile.noTile && Game.currentGame.board.GetEntityAtTile(startingTile) == Entity.noEntity;
            }

            public override void GetTilesAndEntitiesAffected(out Entity[] entitiesAffected, out Tile[] tilesAffected){
                entitiesAffected = new Entity[0];
                tilesAffected = new Tile[1]{Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, associatedEntity.direction)};
            }

            public override string GetEffectText()
            {
                return $"Spawn {scriptableEntity.name}";
            }
        }

    }
}