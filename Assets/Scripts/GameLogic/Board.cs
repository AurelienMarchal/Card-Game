
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic{

    using GameState;
    using GameEffect;
    using System.Linq;

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

        //temp
        public List<Entity> entities {
            get {
                entities_.Clear();
                if (Game.currentGame != null) {
                    foreach (Player player in Game.currentGame.players) {
                        foreach (Entity entity in player.entities) {
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
                    tiles[i*gridHeight + j] = new Tile(i, j, (uint)(i *gridHeight + j));
                }
            }

            //Debug.Log($"Tiles : [{String.Join(", ", (object[])tiles)}]");
        }

        public Tile GetTileWithNum(uint tileNum){
            if(tileNum >= tiles.Length){
                return Tile.noTile;
            }

            return tiles[(int)tileNum];
        }

        public Tile GetTileAt(int gridX, int gridY)
        {
            if (gridX < 0 || gridY < 0 || gridX >= gridHeight || gridY >= gridWidth)
            {
                return Tile.noTile;
            }

            return tiles[gridX * gridHeight + gridY];
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
        
        public Entity[] GetEntitiesAtTiles(Tile[] tiles){

            var currentEntities = entities;

            var entitiesToReturn = new List<Entity>();

            foreach (var entity in currentEntities)
            {
                if (entity.currentTile == Tile.noTile)
                {
                    continue;
                }
                if (tiles.Contains(entity.currentTile))
                {
                    entitiesToReturn.Add(entity);
                }
            }

            return entitiesToReturn.ToArray();
        }

        public Entity GetFirstEntityInDirectionWithRange(Tile startTile, Direction direction, int range, out Tile[] tilesRanged)
        {

            var tile = startTile;

            var tileList = new List<Tile>();

            while (range > 0 && tile != Tile.noTile)
            {
                tileList.Add(tile);
                var entityAtTile = GetEntityAtTile(tile);
                if (entityAtTile != Entity.noEntity)
                {
                    tilesRanged = tileList.ToArray();
                    return entityAtTile;
                }

                tile = NextTileInDirection(tile, direction);
                range--;
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
        
        public Tile[] GetAllTilesInDirectionWithRange(Tile startTile, Direction direction, int range){

            var tile = startTile;

            var tileList = new List<Tile>();

            //Debug.Log($"startTile {tile}, {direction}");

            while(range > 0 && tile != Tile.noTile){
                //Debug.Log($"startTile {tile}, {direction}");
                tileList.Add(tile);

                tile = NextTileInDirection(tile, direction);
                range --;
            }

            return tileList.ToArray();
        }

        public List<Tile> GetTileSquareAroundTile(Tile tile, int squareSize)
        {
            var toReturn = new List<Tile>();

            if (tile == Tile.noTile)
            {
                return toReturn;
            }

            if (squareSize == 0)
            {
                toReturn.Add(tile);
                return toReturn;
            }

            var halfSquareSize = Math.Abs(squareSize) % 2 == 0 ? Math.Abs(squareSize) / 2 : (Math.Abs(squareSize) + 1) / 2;

            for (int i = tile.gridX - halfSquareSize; i < tile.gridX + halfSquareSize; i++)
            {
                for (int j = tile.gridY - halfSquareSize; j < tile.gridY + halfSquareSize; j++)
                {
                    var tileij = GetTileAt(i, j);
                    if (tileij != Tile.noTile)
                    {
                        toReturn.Add(tileij);
                    }
                }
            }

            return toReturn;
        }

        private void SetupPermanentEffects()
        {

        }


        public BoardState ToBoardState(){
            BoardState boardState = new BoardState();
            boardState.gridWidth = gridWidth;
            boardState.gridHeight = gridHeight;
            boardState.tileStates = new List<TileState>();

            foreach (Tile tile in tiles){
                boardState.tileStates.Add(tile.ToTileState());
            }

            boardState.effectStates = new List<EffectState>();

            return boardState;
        }
    }

}