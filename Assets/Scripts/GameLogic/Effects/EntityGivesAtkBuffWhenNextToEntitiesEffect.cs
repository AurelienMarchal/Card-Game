using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic{

    using GameAction;
    using GameBuff;

    namespace GameEffect{
//TODO
        public class EntityGivesAtkBuffWhenNextToEntitiesEffect : EntityEffect{
            public int amount{
                get;
                private set;
            }

            private Entity[] lastEntitiesAffected{
                get;
                set;
            }

            public EntityGivesAtkBuffWhenNextToEntitiesEffect(int amount, Entity entity, bool displayOnUI = true) : base(entity, displayOnUI){
                this.amount = amount;
                entityBuffs.Add(new AtkBuff(amount));
                lastEntitiesAffected = new Entity[0];
            }

            public override bool Trigger(Action action){
                switch (action){
                    case StartGameAction startGameAction:
                        return startGameAction.wasPerformed;

                    case PlayerSpawnEntityAction playerSpawnEntityAction:
                        return playerSpawnEntityAction.entitySpawned == associatedEntity && playerSpawnEntityAction.wasPerformed;

                    case EntityMoveAction entityMoveAction:
                        return entityMoveAction.wasPerformed;

                    case EntityDieAction entityDieAction:
                        return entityDieAction.wasPerformed;
                }

                return false;
            }

            protected override void Activate(){

                GetTilesAndEntitiesAffected(out Entity[] entitiesAffected, out Tile[] _);

                Debug.Log($"{this} lastEntitiesAffected : [{String.Join(", ", (object[])lastEntitiesAffected)}]");

                Debug.Log($"{this} entitiesAffected : [{String.Join(", ", (object[])entitiesAffected)}]");

                foreach (var entity in lastEntitiesAffected){
                    if(entity != null && !entitiesAffected.Contains(entity) && entity.affectedByEffects.Contains(this)){
                        entity.affectedByEffects.Remove(this);
                        entity.UpdateTempBuffsAccordingToEffects();
                    }
                }

                foreach(var entity in entitiesAffected){
                    if(!entity.affectedByEffects.Contains(this)){
                        entity.affectedByEffects.Add(this);
                        entity.UpdateTempBuffsAccordingToEffects();
                    }
                }

                lastEntitiesAffected = (Entity[])entitiesAffected.Clone();
            }

            public override void GetTilesAndEntitiesAffected(out Entity[] entitiesAffected, out Tile[] tilesAffected){

                var tileNorth = Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.North);

                var tileSouth = Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.South);

                var tileEast = Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.East);

                var tileWest = Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.West);

                var tileCount = 0;

                if(tileNorth != Tile.noTile){
                    tileCount ++;
                }
                if(tileNorth != Tile.noTile){
                    tileCount ++;
                }
                if(tileNorth != Tile.noTile){
                    tileCount ++;
                }
                if(tileNorth != Tile.noTile){
                    tileCount ++;
                }

                tilesAffected = new Tile[tileCount];

                var tileIndex = 0;

                if(tileNorth != Tile.noTile){
                    tilesAffected[tileIndex] = tileNorth;
                    tileIndex ++;
                }
                if(tileSouth != Tile.noTile){
                    tilesAffected[tileIndex] = tileSouth;
                    tileIndex ++;
                }
                if(tileEast != Tile.noTile){
                    tilesAffected[tileIndex] = tileEast;
                    tileIndex ++;
                }
                if(tileWest != Tile.noTile){
                    tilesAffected[tileIndex] = tileWest;
                }

                var entityNorth = Game.currentGame.board.GetEntityAtTile(tileNorth);

                var entitySouth = Game.currentGame.board.GetEntityAtTile(tileSouth);

                var entityEast = Game.currentGame.board.GetEntityAtTile(tileEast);

                var entityWest = Game.currentGame.board.GetEntityAtTile(tileWest);

                var entityCount = 0;

                if(entityNorth != Entity.noEntity){
                    entityCount ++;
                }
                if(entitySouth != Entity.noEntity){
                    entityCount ++;
                }
                if(entityEast != Entity.noEntity){
                    entityCount ++;
                }
                if(entityWest != Entity.noEntity){
                    entityCount ++;
                }

                entitiesAffected = new Entity[entityCount];

                var entityIndex = 0;

                if(entityNorth != Entity.noEntity){
                    entitiesAffected[entityIndex] = entityNorth;
                    entityIndex ++;
                }
                if(entitySouth != Entity.noEntity){
                    entitiesAffected[entityIndex] = entitySouth;
                    entityIndex ++;
                }
                if(entityEast != Entity.noEntity){
                    entitiesAffected[entityIndex] = entityEast;
                    entityIndex ++;
                }
                if(entityWest != Entity.noEntity){
                    entitiesAffected[entityIndex] = entityWest;
                }
            }


            public override string GetEffectText(){
                return $"Gives {amount} ATK to entities next to {associatedEntity.name}";
            }


        }
    }
}