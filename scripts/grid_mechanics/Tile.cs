using System.Numerics;
using EngineLearning.scripts.grid_game.misc;

namespace EngineLearning.scripts.grid_mechanics {
    public class Tile {
        internal TileType Type { get; set; }
        internal int X { get; set; }
        internal int Y { get; set; }

        internal int OwnID;

        static internal int ID;
        
        internal TileOrientation Orientation { get; set; }


        public Tile(TileType type, int x, int y) {
            OwnID = ID;
            ID++;
            Type = type;
            X = x;
            Y = y;
            Orientation = TileOrientation.RIGHT;
        }

        public Tile(TileType type, int x, int y, TileOrientation orientation) : this(type, x, y) {
            Orientation = orientation;
        }

        public override int GetHashCode() {
            return OwnID;
        }

        public Vector2Int NextPosition() {
            switch (Orientation) {
                case TileOrientation.UP: return new Vector2Int(X, Y - 1);
                case TileOrientation.DOWN: return new Vector2Int(X, Y + 1);
                case TileOrientation.LEFT: return new Vector2Int(X - 1, Y);
                case TileOrientation.RIGHT: return new Vector2Int(X + 1, Y);
                default: return new Vector2Int(X + 1, Y);
            }
        }

        public override string ToString() {
            return $"tile_x{X}_y{Y}";
        }
    }
}