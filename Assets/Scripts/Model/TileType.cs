public enum TileType{

    Standard, Nature, Cursed, CurseSource, WillGetCursed
    
}


public static class TileTypeExtensions
{
public static string ToTileString(this TileType tileType){
    switch(tileType){
        case TileType.Standard:
            return "Standard Tile";
        case TileType.Nature:
            return "Nature Tile";
        case TileType.Cursed:
            return "Cursed Tile";
        case TileType.CurseSource:
            return "Curse source Tile";
        default:
            return "Tile";
        }  
    }
}

