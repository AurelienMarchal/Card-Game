
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic{

    using GameAction;
    using GameEffect;
    public class Board {

        public int gridHeight{
            get;
            private set;
        }

        public int gridWidth{
            get;
            private set;
        }

        public Tile[] tiles{
            get;
            private set;
        }

        public List<Effect> effects{
            get;
            protected set;
        }

        private List<Entity> entities_;

        public List<Entity> entities{
            get{
                entities_.Clear();
                if(Game.currentGame != null){
                    foreach(Player player in Game.currentGame.players){
                        foreach(Entity entity in player.entities){
                            //Debug.Log($"{player} has {entity}");
                            entities_.Add(entity);
                        }
                    }
                }

                return entities_;
            }
            
        }


        public Board(int gridHeight, int gridWidth){
            this.gridHeight = gridHeight;
            this.gridWidth = gridWidth;

            entities_ = new List<Entity>();

            tiles = new Tile[gridHeight * gridWidth];
            effects = new List<Effect>();
            SetupPermanentEffects();

            for(var i = 0; i < gridWidth; i++){
                for(var j = 0; j < gridHeight; j++){
                    tiles[i*gridHeight + j] = new Tile(i, j, i*gridHeight + j);
                }
            }

            //Debug.Log($"Tiles : [{String.Join(", ", (object[])tiles)}]");
        }

        public Tile GetTileAt(int gridX, int gridY){
            if(gridX < 0 || gridY < 0 || gridX >= gridHeight || gridY >= gridWidth){
                return Tile.noTile;
            }

            return tiles[gridX*gridHeight + gridY];
        }

        public Tile NextTileInDirection(Tile startingTile, Direction direction){

            if(startingTile == Tile.noTile){
                return Tile.noTile;
            }
            
            var incrementX = 0;
            var incrementY = 0;

            switch (direction)
            {

                case Direction.North:
                    incrementX = 0;
                    incrementY = 1;
                    break;

                case Direction.South:
                    incrementX = 0;
                    incrementY = -1;
                    break;

                case Direction.East:
                    incrementX = 1;
                    incrementY = 0;
                    break;

                case Direction.West:
                    incrementX = -1;
                    incrementY = 0;
                    break;

                default:
                    break;
            }

            //Debug.Log($"Starting tile : {startingTile}. Next tile in direction {direction} : {GetTileAt(startingTile.gridX + incrementX, startingTile.gridY + incrementY)};");

            return GetTileAt(startingTile.gridX + incrementX, startingTile.gridY + incrementY);

        }

        public Entity GetEntityAtTile(Tile tile){

            if(tile == Tile.noTile){
                return Entity.noEntity;
            }
            
            foreach (var entity in entities){
                if(entity.currentTile == tile){
                    return entity;
                }
            }

            return Entity.noEntity;
        }

        public Entity GetFirstEntityInDirectionWithRange(Tile startTile, Direction direction, int range, out Tile[] tilesRanged){

            var tile = startTile;

            var tileList = new List<Tile>();

            while(range > 0 && tile != Tile.noTile){
                tileList.Add(tile);
                var entityAtTile = GetEntityAtTile(tile);
                if(entityAtTile != Entity.noEntity){
                    tilesRanged = tileList.ToArray();
                    return entityAtTile;
                }

                tile = NextTileInDirection(tile, direction);
                range --;
            }

            tilesRanged = tileList.ToArray();
            return Entity.noEntity;
        }

        public Entity[] GetAllEntitiesInDirectionWithRange(Tile startTile, Direction direction, int range, out Tile[] tilesRanged){

            var tile = startTile;

            var tileList = new List<Tile>();

            var entityList = new List<Entity>();

            //Debug.Log($"startTile {tile}, {direction}");

            while(range > 0 && tile != Tile.noTile){
                //Debug.Log($"startTile {tile}, {direction}");
                tileList.Add(tile);
                var entityAtTile = GetEntityAtTile(tile);
                if(entityAtTile != Entity.noEntity){
                    entityList.Add(entityAtTile);
                }

                tile = NextTileInDirection(tile, direction);
                range --;
            }

            tilesRanged = tileList.ToArray();
            return entityList.ToArray();
        }

        private void SetupPermanentEffects(){

        }

        
    }

}