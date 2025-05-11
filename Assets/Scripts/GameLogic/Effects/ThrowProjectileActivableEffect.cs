using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameLogic{

    using GameAction;

    namespace GameEffect{
        public class ThrowProjectileActivableEffect : ActivableEffect{

            public Direction direction{
                get;
                protected set;
            }

            public Damage damage{
                get;
                protected set;
            }

            public int range{
                get;
                protected set;
            }
            
            public Entity entityHit{
                get;
                protected set;
            }

            public Tile tileReached{
                get;
                protected set;
            }


            public ThrowProjectileActivableEffect(Entity casterEntity, Cost cost, Damage damage, int range) : base(casterEntity, cost){
                this.damage = damage;
                this.range = range;
                entityHit = Entity.noEntity;
                tileReached = Tile.noTile;
            }

            protected override void Activate(){
                GetTilesAndEntitiesAffected(out Entity[] entitiesAffected, out Tile[] tilesAffected);
                if(entitiesAffected.Length > 0 && entitiesAffected[0] != Entity.noEntity){
                    entityHit = entitiesAffected[0];
                    tileReached = entityHit.currentTile;
                    Game.currentGame.PileAction(new EntityTakeDamageAction(entityHit, damage, effectActivatedAction));
                }
            }


            public override bool CanBeActivated(){
                return base.CanBeActivated() && Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, associatedEntity.direction) != Tile.noTile;
            }

            public override void GetTilesAndEntitiesAffected(out Entity[] entitiesAffected, out Tile[] tilesAffected)
            {
                if(associatedEntity == Entity.noEntity){
                    tilesAffected = new Tile[0];
                    entitiesAffected = new Entity[0];
                    return;
                }

                var nextTile = Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, associatedEntity.direction);
                var entityRanged = Game.currentGame.board.GetFirstEntityInDirectionWithRange(nextTile, associatedEntity.direction, range, out tilesAffected);

                if(entityRanged != Entity.noEntity){
                    entitiesAffected = new Entity[1]{entityRanged};
                    return;
                }
                else{
                    entitiesAffected = new Entity[0];
                    return;
                }
            }

            public override string GetEffectText()
            {
                return $"Deal {damage} with range {range}";
            }
        }
    }
}