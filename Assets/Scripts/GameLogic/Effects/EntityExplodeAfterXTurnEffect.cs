using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace GameLogic{

    using GameAction;

    namespace GameEffect{
        public class EntityExplodeAfterXTurnEffect : EntityEffect, CanBeActivatedInterface, AffectsTilesInterface{


            public int turnLeft{
                get;
                protected set;
            }

            public int range{
                get;
                protected set;
            }

            public Damage damage{
                get;
                protected set;
            }

            List<Tile> tilesInExplosionRange;

            List<Entity> entitiesInExplosionRange;


            public EntityExplodeAfterXTurnEffect(int turnNb, int explosionRange, Damage damage, Entity entity) : base(entity, true)
            {
                turnLeft = turnNb;
                range = explosionRange;
                this.damage = damage;
                entitiesInExplosionRange = new List<Entity>();
                tilesInExplosionRange = new List<Tile>();
            }

            public bool CheckTriggerToActivate(Action action){
                switch (action){
                    case PlayerEndTurnAction playerEndTurnAction:
                        if(playerEndTurnAction.wasPerformed && playerEndTurnAction.player == associatedEntity.player){
                            
                            turnLeft --;
                            //Debug.Log($"Turn Left : {turnLeft}");
                            return turnLeft == 0;
                        }
                        return false;
                }

                return false;
            }

            public void Activate(){
                //Explodes !
                Game.currentGame.PileAction(new EntityDieAction(associatedEntity));

                var actions = new List<Action>();

                
                foreach(Entity entity in entitiesInExplosionRange){
                    actions.Add(new EntityTakeDamageAction(entity, damage));
                }


                Game.currentGame.PileActions(actions.ToArray());
            }

            public Type[] ActionTypeTriggersToActivate()
            {
                return new Type[1]{typeof(PlayerEndTurnAction)};
            }

            public bool CanBeActivated()
            {
                return associatedEntity != Entity.noEntity;
            }

            public override string GetEffectName()
            {
                return "Ticking bomb";
            }

            public override string GetEffectText()
            {
                return $"Explode in {turnLeft} turns !! ";
            }

            public override void UpdateEntitiesAffected()
            {
                entitiesInExplosionRange.Clear();
                entitiesInExplosionRange.AddRange(Game.currentGame.board.GetEntitiesAtTiles(tilesInExplosionRange.ToArray()));
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

            public override List<Entity> GetEntitiesAffected()
            {
                return entitiesInExplosionRange;
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

            public void UpdateTilesAffected()
            {
                tilesInExplosionRange.Clear();
                var tilesNorth = Game.currentGame.board.GetAllTilesInDirectionWithRange(
                    Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.North),
                    Direction.North,
                    range);

                tilesInExplosionRange.AddRange(tilesNorth);

                var tilesSouth = Game.currentGame.board.GetAllTilesInDirectionWithRange(
                    Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.South),
                    Direction.South,
                    range);

                tilesInExplosionRange.AddRange(tilesSouth);

                var tilesEast = Game.currentGame.board.GetAllTilesInDirectionWithRange(
                    Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.East),
                    Direction.East,
                    range);

                tilesInExplosionRange.AddRange(tilesEast);

                var tilesWest = Game.currentGame.board.GetAllTilesInDirectionWithRange(
                    Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.West),
                    Direction.West,
                    range);

                tilesInExplosionRange.AddRange(tilesWest);
            }

            public List<Tile> GetTilesAffected()
            {
                return tilesInExplosionRange;
            }
        }

    }
}