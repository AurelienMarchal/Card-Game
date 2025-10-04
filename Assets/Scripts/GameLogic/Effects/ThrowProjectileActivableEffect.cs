using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameLogic{

    using GameAction;

    namespace GameEffect{
        public class ThrowProjectileEntityEffect : EntityEffect, AffectsTilesInterface, CanBeActivatedInterface
        {

            public Direction direction
            {
                get;
                protected set;
            }

            public Damage damage
            {
                get;
                protected set;
            }

            public int range
            {
                get;
                protected set;
            }

            public Entity entityHit
            {
                get;
                protected set;
            }

            public Tile tileReached
            {
                get;
                protected set;
            }

            List<Tile> tilesInRange;

            List<Entity> entitiesInRange;


            public ThrowProjectileEntityEffect(Entity casterEntity, Damage damage, int range) : base(casterEntity)
            {
                this.damage = damage;
                this.range = range;
                entityHit = Entity.noEntity;
                tileReached = Tile.noTile;
                tilesInRange = new List<Tile>();
                entitiesInRange = new List<Entity>();
            }

            void CanBeActivatedInterface.Activate()
            {
                
                if (entitiesInRange.Count > 0 && entitiesInRange[0] != Entity.noEntity)
                {
                    entityHit = entitiesInRange[0];
                    tileReached = entityHit.currentTile;
                    Game.currentGame.PileAction(new EntityTakeDamageAction(entityHit, damage));
                }
            }


            public bool CanBeActivated()
            {
                return associatedEntity != Entity.noEntity && Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, associatedEntity.direction) != Tile.noTile;
            }

            public bool CheckTriggerToActivate(Action action)
            {
                return false;
            }

            public override string GetEffectText()
            {
                return $"Deal {damage} with range {range}";
            }

            public bool CheckTriggerToUpdateTilesAffected(Action action)
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

            public void UpdateTilesAffected()
            {
                tilesInRange.Clear();
                entitiesInRange.Clear();

                var nextTile = Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, associatedEntity.direction);
                var entityRanged = Game.currentGame.board.GetFirstEntityInDirectionWithRange(nextTile, associatedEntity.direction, range, out Tile[] tilesRanged);
                tilesInRange.AddRange(tilesRanged);

                if (entityRanged != Entity.noEntity)
                { 
                    entitiesInRange.Add(entityRanged);
                }
                
            }

            public List<Tile> GetTilesAffected()
            {
                return tilesInRange;
            }
            
            public override bool CheckTriggerToUpdateEntitiesAffected(Action action)
            {
                return false;
            }

            public override void UpdateEntitiesAffected()
            {
                
            }

            public override List<Entity> GetEntitiesAffected()
            {
                return entitiesInRange;
            }

            
        }
    }
}