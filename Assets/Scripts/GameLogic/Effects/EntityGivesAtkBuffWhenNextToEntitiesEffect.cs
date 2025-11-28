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
        public class EntityGivesAtkBuffWhenNextToEntitiesEffect : EntityEffect, AffectsEntitiesInterface, AffectsTilesInterface, GivesTempBuffInterface{
            public int amount{
                get;
                private set;
            }

            private List<Entity> entitiesAffected;
            private List<Tile> tilesAffected;

            private List<Buff> tempBuffs;

            public EntityGivesAtkBuffWhenNextToEntitiesEffect(int amount, Entity entity, bool displayOnUI = true) : base(entity, displayOnUI)
            {
                this.amount = amount;

                tempBuffs = new List<Buff>
                {
                    new AtkBuff(amount, id.ToString())
                };

                entitiesAffected = new List<Entity>();
                tilesAffected = new List<Tile>();
            }


            public void UpdateTilesAffected()
            {
                tilesAffected.Clear();
                var tileNorth = Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.North);

                var tileSouth = Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.South);

                var tileEast = Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.East);

                var tileWest = Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.West);

                if (tileNorth != Tile.noTile)
                {
                    tilesAffected.Add(tileNorth);
                }
                if (tileSouth != Tile.noTile)
                {
                    tilesAffected.Add(tileSouth);
                }
                if (tileEast != Tile.noTile)
                {
                    tilesAffected.Add(tileEast);
                }
                if (tileWest != Tile.noTile)
                {
                    tilesAffected.Add(tileWest);
                }
            }
            
            public override void UpdateEntitiesAffected()
            {
                
                entitiesAffected.Clear();

                foreach (var tile in tilesAffected)
                {
                    var entity = Game.currentGame.board.GetEntityAtTile(tile);
                    if (entity != Entity.noEntity)
                    {
                        entitiesAffected.Add(entity);
                    }
                }
            }
            
            public void UpdateTempBuffs()
            {
                
            }

            public override string GetEffectText()
            {
                return $"Gives {amount} ATK to entities next to {associatedEntity.name}";
            }

            public override List<Entity> GetEntitiesAffected()
            {
                return entitiesAffected;
            }

            public List<Tile> GetTilesAffected()
            {
                return tilesAffected;
            }

            public List<Buff> GetTempBuffs()
            {
                return tempBuffs;
            }


            //Not necessary
            public override bool CheckTriggerToUpdateEntitiesAffected(Action action)
            {
                switch (action)
                {
                    case PlayerSpawnEntityAction playerSpawnEntityAction:
                        return playerSpawnEntityAction.wasPerformed;
                    case EntityMoveAction entityMoveAction:
                        return entityMoveAction.wasPerformed;
                    case EntityDieAction entityDieAction:
                        return entityDieAction.wasPerformed;
                }

                return false;
            }

            public override System.Type[] ActionTypeTriggersToUpdateEntitiesAffected()
            {
                return new Type[3]{typeof(PlayerSpawnEntityAction), typeof(EntityMoveAction), typeof(EntityDieAction)};
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

            public Type[] ActionTypeTriggersToUpdateTilesAffected()
            {
                return new Type[3]{typeof(PlayerSpawnEntityAction), typeof(EntityMoveAction), typeof(EntityDieAction)};
            }

            public bool CheckTriggerToUpdateTempBuffs(Action action)
            {
                return false;
            }

            public Type[] ActionTypeTriggersToUpdateTempBuffs()
            {
                return null;
            }
        }
    }
}