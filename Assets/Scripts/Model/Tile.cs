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

    public List<Effect> effects;

    public Tile(int gridX, int gridY, int num, TileType tileType = TileType.Standard){
        this.gridX = gridX;
        this.gridY = gridY;
        this.num = num;
        this.tileType = tileType;
        effects = new List<Effect>();
    }

    public int Distance(Tile tile){
        return Math.Abs(gridX - tile.gridX) + Math.Abs(gridY - tile.gridY);
    }

    

}


