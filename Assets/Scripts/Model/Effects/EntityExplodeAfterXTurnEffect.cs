using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace GameLogic{

    using GameAction;

    namespace GameEffect{
        public class EntityExplodeAfterXTurnEffect : EntityEffect{


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


            public EntityExplodeAfterXTurnEffect(int turnNb, int explosionRange, Damage damage, Entity entity) : base(entity, true){
                turnLeft = turnNb;
                range = explosionRange;
                this.damage = damage;
            }

            public override bool Trigger(Action action){
                switch (action){
                    case EndPlayerTurnAction endPlayerTurnAction:
                        if(endPlayerTurnAction.wasPerformed && endPlayerTurnAction.player == associatedEntity.player){
                            
                            turnLeft --;
                            Debug.Log($"Turn Left : {turnLeft}");
                            return turnLeft == 0;
                        }
                        return false;
                }

                return false;
            }

            protected override void Activate(){

                Game.currentGame.PileAction(new EntityDieAction(associatedEntity, effectActivatedAction));

                var actions = new List<Action>();

                GetTilesAndEntitiesAffected(out Entity[] entitiesAffected, out Tile[] _);
                foreach(Entity entity in entitiesAffected){
                    actions.Add(new EntityTakeDamageAction(entity, damage, effectActivatedAction));
                    
                }

                Game.currentGame.PileActions(actions.ToArray());

                
                
            }

            public override string GetEffectText(){
                return $"Explode in {turnLeft} turns !! ";
            }


            public override void GetTilesAndEntitiesAffected(out Entity[] entitiesAffected, out Tile[] tilesAffected){
                var entitiesNorth = Game.currentGame.board.GetAllEntitiesInDirectionWithRange(
                    Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.North),
                    Direction.North,
                    range,
                    out Tile[] tilesNorth);

                var entitiesSouth = Game.currentGame.board.GetAllEntitiesInDirectionWithRange(
                    Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.South),
                    Direction.South,
                    range,
                    out Tile[] tilesSouth);

                var entitiesEast = Game.currentGame.board.GetAllEntitiesInDirectionWithRange(
                    Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.East),
                    Direction.East,
                    range,
                    out Tile[] tilesEast);

                var entitiesWest = Game.currentGame.board.GetAllEntitiesInDirectionWithRange(
                    Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.West),
                    Direction.West,
                    range,
                    out Tile[] tilesWest);

                var entitiesAffectedDynamicList = new List<Entity>();
                
                entitiesAffectedDynamicList.AddRange(entitiesNorth);
                entitiesAffectedDynamicList.AddRange(entitiesSouth);
                entitiesAffectedDynamicList.AddRange(entitiesEast);
                entitiesAffectedDynamicList.AddRange(entitiesWest);

                entitiesAffected = entitiesAffectedDynamicList.ToArray();

                var tilesAffectedDynamicList = new List<Tile>();

                tilesAffectedDynamicList.AddRange(tilesNorth);
                tilesAffectedDynamicList.AddRange(tilesSouth);
                tilesAffectedDynamicList.AddRange(tilesEast);
                tilesAffectedDynamicList.AddRange(tilesWest);

                tilesAffected = tilesAffectedDynamicList.ToArray();
                
            }
        }

    }
}