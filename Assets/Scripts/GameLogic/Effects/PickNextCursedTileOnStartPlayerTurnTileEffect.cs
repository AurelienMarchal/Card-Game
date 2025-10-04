using System;
using System.Collections.Generic;
using UnityEngine;


namespace GameLogic{

    using GameAction;

    namespace GameEffect{
        public class PickNextCursedTileOnStartPlayerTurnTileEffect : TileEffect, CanBeActivatedInterface
        {
            List<Tile> possibleTiles;

            public PickNextCursedTileOnStartPlayerTurnTileEffect(Tile tile) : base(tile)
            {
                possibleTiles = new List<Tile>();
            }

            void CanBeActivatedInterface.Activate()
            {
                var randomIndex = Game.currentGame.random.Next(possibleTiles.Count);
                Game.currentGame.PileAction(new TileChangeTypeAction(possibleTiles[randomIndex], TileType.WillGetCursed));
            }

            public bool CanBeActivated()
            {
                return associatedTile != Tile.noTile && associatedTile.tileType == TileType.CurseSource && possibleTiles.Count > 0;
            }

            public bool CheckTriggerToActivate(Action action)
            {
                switch (action)
                {
                    case PlayerStartTurnAction playerStartTurnAction:
                        return playerStartTurnAction.wasPerformed;
                }

                return false;
            }

            public override bool CheckTriggerToUpdateTilesAffected(Action action)
            {

                if (associatedTile.tileType != TileType.CurseSource)
                {
                    return false;
                }
                
                switch (action)
                {
                    case TileChangeTypeAction tileChangeTypeAction:
                        return tileChangeTypeAction.wasPerformed;
                }
                return false;
            }

            public override List<Tile> GetTilesAffected()
            {
                return possibleTiles;
            }

            public override void UpdateTilesAffected()
            {
                //Check if the tile is adjacent of a source
                possibleTiles.Clear();

                foreach (var tile in Game.currentGame.board.tiles)
                {
                    if (tile.tileType != TileType.Cursed && tile.tileType != TileType.WillGetCursed && tile.tileType != TileType.CurseSource)
                    {
                        var tileNorth = Game.currentGame.board.NextTileInDirection(tile, Direction.North);
                        if (tileNorth != Tile.noTile)
                        {
                            if (tileNorth.tileType == TileType.Cursed || tileNorth.tileType == TileType.CurseSource)
                            {
                                possibleTiles.Add(tile);
                                continue;
                            }
                        }

                        var tileSouth = Game.currentGame.board.NextTileInDirection(tile, Direction.South);
                        if (tileSouth != Tile.noTile)
                        {
                            if (tileSouth.tileType == TileType.Cursed || tileSouth.tileType == TileType.CurseSource)
                            {
                                possibleTiles.Add(tile);
                                continue;
                            }
                        }

                        var tileEast = Game.currentGame.board.NextTileInDirection(tile, Direction.East);
                        if (tileEast != Tile.noTile)
                        {
                            if (tileEast.tileType == TileType.Cursed || tileEast.tileType == TileType.CurseSource)
                            {
                                possibleTiles.Add(tile);
                                continue;
                            }
                        }

                        var tileWest = Game.currentGame.board.NextTileInDirection(tile, Direction.West);
                        if (tileWest != Tile.noTile)
                        {
                            if (tileWest.tileType == TileType.Cursed || tileWest.tileType == TileType.CurseSource)
                            {
                                possibleTiles.Add(tile);
                                continue;
                            }
                        }
                    }
                }
            }
        }
    }


}