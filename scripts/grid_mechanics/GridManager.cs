using System;
using System.Collections.Generic;
using System.Linq;
using EngineLearning.scripts.grid_game.misc;
using Godot;
using Vector2 = System.Numerics.Vector2;

namespace EngineLearning.scripts.grid_mechanics {
    
    public delegate void TileSpawnedHandler(object sender, TileEventArgs args);

    
    internal class GridManager : Node {
        private int _x;
        private int _y;

        /*
         * Two-dimensional byte array indicating if a position
         * is currently occupied by a tile or not.
         *
         * A value != 0 indicates that the position is occupied.
         */
        private byte[,] occupiedBoard;

        internal List<Tile> MoveableTiles { get; }
        internal List<Tile> StationaryTiles { get; }

        internal IntVec2[] neigborOffsets = {
            new IntVec2(1,0),        // Right
            new IntVec2(-1, 0),      // Left
            new IntVec2(0, 1),       // Down
            new IntVec2(0, -1)       // Up
        };
        
        
        public event TileSpawnedHandler TileSpawnedEvent;

        internal GridManager(int x, int y) {

            MoveableTiles = new List<Tile>();
            StationaryTiles = new List<Tile>();
            
            occupiedBoard = new byte[y, x];
            _x = x;
            _y = y;
        }

        /*
         * Adds a new tile with the given type to the gameboard.
         */
        internal void AddTile(TileType type, int x, int y) {
            if (occupiedBoard[y, x] == 0) {
                Tile tile = new Tile(type, x, y);
                occupiedBoard[y, x] = 1;
                switch (tile.Type) {
                    case TileType.MOVEABLE_TILE:
                        MoveableTiles.Add(tile);
                        break;
                    case TileType.ROTATE_RIGHT_TILE:
                    case TileType.SPAWNER_TILE:
                    case TileType.WOOD_TILE:
                        StationaryTiles.Add(tile);
                        break;
                    default: break;
                }
            }
        }

        internal Tile AddTile(TileType type, TileOrientation orientation, int x, int y) {
            if (occupiedBoard[y, x] == 0) {
                Tile tile = new Tile(type, x, y, orientation);
                occupiedBoard[y, x] = 1;
                switch (tile.Type) {
                    case TileType.MOVEABLE_TILE:
                        MoveableTiles.Add(tile);
                        break;
                    case TileType.ROTATE_RIGHT_TILE:
                    case TileType.SPAWNER_TILE:
                    case TileType.WOOD_TILE:
                        StationaryTiles.Add(tile);
                        break;
                    default: break;
                }

                return tile;
            }

            return null;
        }

        internal void UpdateBoard() {

            MoveableTiles.ForEach(tile => {
                IntVec2 nextPos = tile.NextPosition();

                
                // If at the next position is no tile then move
                if (IsValidCoordinate(nextPos) && 
                    occupiedBoard[nextPos.Y, nextPos.X] == 0) {
                    
                    // Set old position to unoccupied
                    occupiedBoard[tile.Y, tile.X] = 0;
                    
                    tile.X = nextPos.X;
                    tile.Y = nextPos.Y;
                    
                    // Set new position to occupied
                    occupiedBoard[tile.Y, tile.X] = 1;
                }
            });
            
            StationaryTiles.ForEach(tile => {

                switch (tile.Type) {
                    // Rotate all moveable tiles in neighborhood to right
                    case TileType.ROTATE_RIGHT_TILE:
                        RotateNeighborsRight(tile.X, tile.Y);
                        break;
                    // Spawn New Tile Behind spawner
                    case TileType.SPAWNER_TILE:
                        SpawnNewTile(tile.X, tile.Y, tile.Orientation);
                        break;
                    // Move tile if other moveable tiles are pushing
                    case TileType.WOOD_TILE:

                        foreach (var orientation in TileOrientation.GetValues(typeof(TileOrientation))) {
                            var neighborPosition = ((TileOrientation) orientation).AddOffset(tile.X, tile.Y);

                            if (IsValidCoordinate(neighborPosition) &&
                                occupiedBoard[neighborPosition.Y, neighborPosition.X] != 0) {

                                Tile nTile;

                                if ((nTile = MoveableTiles.Find(x =>
                                    x.X == neighborPosition.X && x.Y == neighborPosition.Y)) != null) {
                                    var nTileN = nTile.Orientation.AddOffset(tile.X, tile.Y);
                                }
                            }
                        }

                        break;
                    default: throw new Exception("Case cannot be reached");
                }
            });
        }

        private bool IsValidCoordinate(IntVec2 pos) {
            return (0 <= pos.X && pos.X < _x) &&
                   (0 <= pos.Y && pos.Y < _y);
        }

        private void RotateNeighborsRight(int x, int y) {
            MoveableTiles.FindAll(tile => {
                    
                    // Check left position
                    if (tile.X == x - 1 && tile.Y == y) {
                        return true;
                        // Check right position
                    } else if (tile.X == x + 1 && tile.Y == y) {
                        return true;
                        // Check up position
                    } else if (tile.X == x && tile.Y == y - 1) {
                        return true;
                        // Check down position
                    } else if (tile.X == x && tile.Y == y + 1) {
                        return true;
                    }
                    else {
                        return false;
                    }

                })
                .ForEach(tile => {
                    tile.Orientation = tile.Orientation.RotateRight();
                });
        }

        private void SpawnNewTile(int x, int y, TileOrientation orientation) {
            var spawnPosition = orientation.GetNeighbor();
            spawnPosition = new IntVec2(spawnPosition.X + x, spawnPosition.Y + y);

            var neighborPosition = orientation.Opposite().GetNeighbor();
            neighborPosition = new IntVec2(neighborPosition.X + x, neighborPosition.Y + y);
            
            if (IsValidCoordinate(spawnPosition) && IsValidCoordinate(neighborPosition) 
                && (occupiedBoard[neighborPosition.Y, neighborPosition.X] != 0) && occupiedBoard[spawnPosition.Y, spawnPosition.X] == 0) {
                
                GD.Print($"Spawning tile at x:{spawnPosition.X}, y:{spawnPosition.Y}");
                
                // Retrieve tile at neighbor position
                var neighborTile = MoveableTiles.FindAll(tile => tile.X == neighborPosition.X && tile.Y == neighborPosition.Y)[0];
                var newTile = AddTile(neighborTile.Type, neighborTile.Orientation, spawnPosition.X, spawnPosition.Y);

                OnTileSpawned(new TileEventArgs(newTile));
            }
        }

        private void OnTileSpawned(TileEventArgs args) {
            if (TileSpawnedEvent != null) {
                TileSpawnedEvent(this, args);
            }
        }
    }
}