using System;
using System.Collections.Generic;
using EngineLearning.scripts.grid_game.misc;
using Godot;

namespace EngineLearning.scripts.grid_game {
    public class Board {
        private Tile[,] cells;

        private List<Tile> tiles;

        private static int tileId;


        private int[,] surroundingPos = {
            {1, 0},
            {-1, 0},
            {0, 1},
            {0, -1}
        };

        public Board(int cellsX, int cellsY) {
            cells = new Tile[cellsY, cellsX];
            tiles = new List<Tile>();

            tileId = 1;
        }

        public void SetMoveableBrick(int x, int y, TileOrientation orientation) {
            if (cells[y, x] != null) {
                throw new ArgumentException($"Tile at position {x},{y} is not null.");
            }

            cells[y, x] = new MoveableTile(x, y, orientation);
            cells[y, x].Id = tileId;

            tiles.Add(cells[y, x]);

            tileId++;
        }

        public void SetRotationBrick(int x, int y, int amount, RotationDirection direction) {
            if (cells[y, x] != null) {
                throw new ArgumentException($"Tile at position {x},{y} is not null.");
            }

            cells[y, x] = new RotationTile(x, y, amount, direction);
            cells[y, x].Id = tileId;

            tiles.Add(cells[y, x]);

            tileId++;
        }

        public void SetSpawnerBrick(int x, int y, TileOrientation orientation) {
            if (cells[y, x] != null) {
                throw new ArgumentException($"Tile at position {x},{y} is not null.");
            }

            cells[y, x] = new SpawnerTile(x, y, orientation);
            cells[y, x].Id = tileId;

            tiles.Add(cells[y, x]);

            tileId++;
        }

        public void Update() {
            for (int i = 0; i < tiles.Count; i++) {
                UpdateTile(tiles[i]);
            }
            
            UpdateSpawner();
        }

        private void UpdateSpawner() {

            List<Tile> newTiles = new List<Tile>();
            
            foreach (var tile in tiles) {
                switch (tile.GetTileType()) {
                    case TileTypes.SPAWNER:
                        SpawnerTile sTile = tile as SpawnerTile;

                        IntVec2 spawnOffset = sTile.Orientation.GetDirectNeighborFromOrientation();

                        int spawnX = sTile.X + spawnOffset.X;
                        int spawnY = sTile.Y + spawnOffset.Y;

                        IntVec2 sourceOffset = sTile.Orientation.GetOppositeOrientation().GetDirectNeighborFromOrientation();
                        int sourceX = sTile.X + sourceOffset.X;
                        int sourceY = sTile.Y + sourceOffset.Y;

                        if (IsInsideBoard(spawnX, spawnY) && cells[spawnY, spawnX] == null) {
                            if (cells[spawnY, spawnX] != null) {
                                throw new ArgumentException($"Tile at position {spawnX},{spawnY} is not null.");
                            }

                            if (IsInsideBoard(sourceX, sourceY) && cells[sourceY, sourceX] != null) {

                                Tile t = cells[sourceY, sourceX].Copy();

                                cells[spawnY, spawnX] = t;
                                cells[spawnY, spawnX].Id = tileId;

                                newTiles.Add(cells[spawnY, spawnX]);

                                tileId++;
                            }
                        }

                        break;
                    default:
                        break;
                }
            }
            
            tiles.AddRange(newTiles);
        }

        private void UpdateTile(Tile tile) {
            switch (tile.GetTileType()) {
                case TileTypes.EMPTY: break;
                case TileTypes.SPAWNER:
                    break;
                case TileTypes.MOVEABLE:
                    MoveableTile mTile = tile as MoveableTile;

                    int newX = mTile.X;
                    int newY = mTile.Y;

                    int angle = mTile.Orientation.GetDegreeFromOrientation();

                    // First determine new direction (in case a rotation block is near by)
                    for (int i = 0; i < 4; i++) {
                        int neighborPosX = newX + surroundingPos[i, 0];
                        int neighborPosY = newY + surroundingPos[i, 1];

                        if (IsInsideBoard(neighborPosX, neighborPosY)
                            && cells[neighborPosY, neighborPosX] != null
                            && cells[neighborPosY, neighborPosX] is RotationTile rotTile) {
                            angle += rotTile.GetRotationInDegree();
                        }
                    }

                    mTile.Orientation = TileOrientationExtension.GetOrientationFromDegree(angle);

                    switch (mTile.Orientation) {
                        case TileOrientation.Left:
                            newX--;
                            break;
                        case TileOrientation.Right:
                            // @TODO: hier Fehler für Bretter mit nicht derselben Dimension
                            newX++;
                            break;

                        case TileOrientation.Down:
                            newY++;
                            break;
                        case TileOrientation.Up:

                            newY--;
                            break;
                    }

                    if (IsInsideBoard(newX, newY) && (cells[newY, newX] == null)) {

                        cells[mTile.Y, mTile.X] = null;
                        
                        mTile.X = newX;
                        mTile.Y = newY;

                        cells[newY, newX] = tile;
                    }

                    break;
                default:
                    break;
            }
        }

        public void GetPosOfTileById(out int x, out int y, int id) {
            Tile tile = tiles.Find(x => x.Id == id);

            if (tile == null) {
                x = y = -1;
            }
            else {
                x = tile.X;
                y = tile.Y;
            }
        }

        public Tile GetTile(int x, int y) {
            return cells[y, x];
        }

        public List<Tile> GetTiles() {
            return tiles;
        }

        private bool IsInsideBoard(int x, int y) {
            return (x >= 0 && x < 8) && (y >= 0 && y < 8);
        }

        public TileOrientation GetOrientationOfTileById(int id) {
            Tile tile = tiles.Find(x => x.Id == id);

            if (tile == null) {
                return TileOrientation.Up;
            }
            else {
                if (tile is MoveableTile mTile ) {
                    return mTile.Orientation;
                }

                if (tile is SpawnerTile sTile) {
                    return sTile.Orientation;
                }

                return TileOrientation.Up;
            }
        }
    }
}