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

    public List<Effect> effects{
        get;
        protected set;
    }

    public List<Entity> entities{
        get{
            List<Entity> entities_ = new List<Entity>();
            if(Game.currentGame != null){
                foreach(Player player in Game.currentGame.players){
                    foreach(Entity entity in player.entities){
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

        tiles = new Tile[gridHeight * gridWidth];
        effects = new List<Effect>();
        SetupPermanentEffects();

        for(var i = 0; i < gridWidth; i++){
            for(var j = 0; j < gridHeight; j++){
                tiles[i*gridHeight + j] = new Tile(i, j, i*gridHeight + j);
            }
        }
    }

    public Tile GetTileAt(int gridX, int gridY){
        if(gridX*gridHeight + gridY >= tiles.Length || gridX < 0 || gridY < 0){
            return Tile.noTile;
        }

        return tiles[gridX*gridHeight + gridY];
    }

    public Tile NextTileInDirection(Tile startingTile, Direction direction){
        
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

        return GetTileAt(startingTile.gridX + incrementX, startingTile.gridY + incrementY);

    }

    public Entity GetEntityAtTile(Tile tile){
        
        foreach (var entity in entities){
            if(entity.currentTile == tile){
                return entity;
            }
        }

        return Entity.noEntity;
    }

    public Entity GetFirstEntityInDirectionWithRange(Tile startTile, Direction direction, int range){
        
        var tile = startTile;

        while(range > 0 && tile != Tile.noTile){
            var entityAtTile = GetEntityAtTile(tile);
            if(entityAtTile != Entity.noEntity){
                return entityAtTile;
            }

            tile = NextTileInDirection(startTile, direction);
            range --;
        }

        return Entity.noEntity;
    }

    private void SetupPermanentEffects(){

    }

    
}
