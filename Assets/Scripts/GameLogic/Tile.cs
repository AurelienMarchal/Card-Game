using System;
using System.Collections.Generic;

namespace GameLogic{


    using GameEffect;
    using GameState;

    public class Tile {

        public int gridX{
            get;
            private set;
        }
        public int gridY{
            get;
            private set;
        }

        public uint num{
            get;
            private set;
        }
        
        public TileType tileType{
            get;
            set;
        }

        public const Tile noTile = null;

        public Tile(int gridX, int gridY, uint num, TileType tileType = TileType.Standard){
            this.gridX = gridX;
            this.gridY = gridY;
            this.num = num;
            this.tileType = tileType;
        }

        public int Distance(Tile tile){
            return Math.Abs(gridX - tile.gridX) + Math.Abs(gridY - tile.gridY);
        }


        public override string ToString()
        {
            return $"Tile {gridX}, {gridY}";
        }

        public TileState ToTileState(){
            TileState tileState = new TileState();

            tileState.gridX = gridX;
            tileState.gridY = gridY;
            tileState.tileType = tileType;
            tileState.num = num;
            tileState.effectStates = new List<EffectState>();
            

            return tileState;
        }

    }

}


