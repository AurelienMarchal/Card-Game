using System;
using System.Collections.Generic;

namespace GameLogic{


    using GameEffect;
    public class Tile {

        public int gridX{
            get;
            private set;
        }
        public int gridY{
            get;
            private set;
        }

        public int num{
            get;
            private set;
        }
        
        public TileType tileType{
            get;
            set;
        }

        public const Tile noTile = null;

        public List<Effect> effects{
            get;
            protected set;
        }

        public Tile(int gridX, int gridY, int num, TileType tileType = TileType.Standard){
            this.gridX = gridX;
            this.gridY = gridY;
            this.num = num;
            this.tileType = tileType;
            effects = new List<Effect>();
            AddDefaultPermanentEffects();
        }

        public int Distance(Tile tile){
            return Math.Abs(gridX - tile.gridX) + Math.Abs(gridY - tile.gridY);
        }

        
        protected void AddDefaultPermanentEffects(){
            effects.Add(new PickNextCursedTileOnStartPlayerTurnTileEffect(this));
            effects.Add(new ChangeWillGetCurseTypeIntoCursedTileEffect(this));
            effects.Add(new CurseTileGivesCurseHeartEffect(this));
            effects.Add(new CurseSourceTileGivesCurseHeartEffect(this));
            effects.Add(new NatureTileGivesNatureHeartEffect(this));
        }

        public override string ToString()
        {
            return $"Tile {gridX}, {gridY}";
        }

    }

}


