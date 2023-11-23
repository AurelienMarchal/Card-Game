using System;

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


    public Tile(int gridX, int gridY, int num){
        this.gridX = gridX;
        this.gridY = gridY;
        this.num = num;
    }

    public int Distance(Tile tile){
        return Math.Abs(gridX - tile.gridX) + Math.Abs(gridY - tile.gridY);
    }

}


