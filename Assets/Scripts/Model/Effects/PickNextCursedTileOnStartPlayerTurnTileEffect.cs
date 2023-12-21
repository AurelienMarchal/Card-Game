using System;
using System.Collections.Generic;
using UnityEngine;

public class PickNextCursedTileOnStartPlayerTurnTileEffect : TileEffect {

    public PickNextCursedTileOnStartPlayerTurnTileEffect(Tile tile) : base(tile){
    
    }

    protected override void Activate(){

        var possibleTiles = GetEveryPossibleTile();
        var randomIndex = Game.currentGame.random.Next(possibleTiles.Count);
        Game.currentGame.PileAction(new TileChangeTypeAction(possibleTiles[randomIndex], TileType.WillGetCursed, effectActivatedAction), false);
    }

    public override bool CanBeActivated()
    {
        return base.CanBeActivated() && associatedTile.tileType == TileType.CurseSource && GetEveryPossibleTile().Count > 0;
    }

    public override bool Trigger(Action action)
    {
        switch(action){
            case StartPlayerTurnAction startPlayerTurnAction:
                    return startPlayerTurnAction.wasPerformed;
                

            default : return false;
        }
    }

    private List<Tile> GetEveryPossibleTile(){
        //Check if the tile is adjacent of a source
        var tiles = new List<Tile>();

        foreach(var tile in Game.currentGame.board.tiles){
            if(tile.tileType != TileType.Cursed && tile.tileType != TileType.WillGetCursed && tile.tileType != TileType.CurseSource){
                var tileNorth = Game.currentGame.board.NextTileInDirection(tile, Direction.North);
                if(tileNorth != Tile.noTile){
                    if(tileNorth.tileType == TileType.Cursed || tileNorth.tileType == TileType.CurseSource){
                        tiles.Add(tile);
                        continue;
                    }
                }

                var tileSouth = Game.currentGame.board.NextTileInDirection(tile, Direction.South);
                if(tileSouth != Tile.noTile){
                    if(tileSouth.tileType == TileType.Cursed || tileSouth.tileType == TileType.CurseSource){
                        tiles.Add(tile);
                        continue;
                    }
                }

                var tileEast = Game.currentGame.board.NextTileInDirection(tile, Direction.East);
                if(tileEast != Tile.noTile){
                    if(tileEast.tileType == TileType.Cursed || tileEast.tileType == TileType.CurseSource){
                        tiles.Add(tile);
                        continue;
                    }
                }

                var tileWest = Game.currentGame.board.NextTileInDirection(tile, Direction.West);
                if(tileWest != Tile.noTile){
                    if(tileWest.tileType == TileType.Cursed || tileWest.tileType == TileType.CurseSource){
                        tiles.Add(tile);
                        continue;
                    }
                }
            }
        }

        return tiles;
    }


}