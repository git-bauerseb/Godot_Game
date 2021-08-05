using System;
using System.Collections.Generic;
using EngineLearning.scripts.grid_game.misc;
using Godot;
using Vector2 = System.Numerics.Vector2;

namespace EngineLearning.scripts.grid_mechanics {
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
                        StationaryTiles.Add(tile);
                        break;
                    default: break;
                }
            }
        }

        internal void UpdateBoard() {
            
            StationaryTiles.ForEach(tile => {

                switch (tile.Type) {
                    // Rotate all moveable tiles in neighborhood to right
                    case TileType.ROTATE_RIGHT_TILE:
                        RotateNeighborsRight(tile.X, tile.Y);
                        break;
                    default: throw new Exception("Case cannot be reached");
                }
            });
            
            MoveableTiles.ForEach(tile => {
                Vector2Int nextPos = tile.NextPosition();

                
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
        }

        private bool IsValidCoordinate(Vector2Int pos) {
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
    }
}