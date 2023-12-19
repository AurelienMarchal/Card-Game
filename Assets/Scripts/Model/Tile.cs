using System;
using System.Collections.Generic;

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
        SetupPermanentEffects();
    }

    public int Distance(Tile tile){
        return Math.Abs(gridX - tile.gridX) + Math.Abs(gridY - tile.gridY);
    }

    private void SetupPermanentEffects(){
        effects.Add(new PickNextCursedTileOnStartPlayerTurnEffect(this));
        effects.Add(new ChangeWillGetCurseTypeIntoCursedTileEffect(this));
    }

}




