using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileChangeTypeAction : TileAction
{
    public TileType newType;

    public TileChangeTypeAction(Tile tile, TileType tileType) : base(tile){
        newType = tileType;
    }

    public override void Perform(){
        tile.tileType = newType;
    }
}
