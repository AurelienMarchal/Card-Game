using System.Collections;
using System.Collections.Generic;

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

    public List<Entity> entities{
        get;
        private set;
    }


    public Board(int gridHeight, int gridWidth){
        this.gridHeight = gridHeight;
        this.gridWidth = gridWidth;

        tiles = new Tile[gridHeight * gridWidth];

        for(var i = 0; i < gridWidth; i++){
            for(var j = 0; j < gridHeight; j++){
                tiles[i*gridHeight + j] = new Tile(i, j, i*gridHeight + j);
            }
        }

        entities = new List<Entity>();
    }

    public Tile GetTileAt(int gridX, int gridY){
        if(gridX*gridHeight + gridY >= tiles.Length || gridX < 0 || gridY < 0){
            return null;
        }

        return tiles[gridX*gridHeight + gridY];
    }
    
}
