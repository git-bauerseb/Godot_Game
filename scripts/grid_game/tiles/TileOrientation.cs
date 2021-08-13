using System.Numerics;
using EngineLearning.scripts.grid_game.misc;
using Vector2 = Godot.Vector2;

namespace EngineLearning.scripts.grid_game {
    public enum TileOrientation {
        Up,
        Down,
        Right,
        Left
    }

    public static class TileOrientationExtension {
        public static int GetDegreeFromOrientation(this TileOrientation orientation) {
            switch (orientation) {
                case TileOrientation.Up: return 0;
                case TileOrientation.Down: return 180;
                case TileOrientation.Left: return 270;
                case TileOrientation.Right: return 90;
                default: return 0;
            }
        }
        
        public static IntVec2 GetDirectNeighborFromOrientation(this TileOrientation orientation) {
            switch (orientation) {
                case TileOrientation.Up: return new IntVec2(0, -1);
                case TileOrientation.Down: return new IntVec2(0, 1);
                case TileOrientation.Left: return new IntVec2(-1, 0);
                case TileOrientation.Right: return new IntVec2(1, 0);
                default: return new IntVec2(0,1);
            }
        }

        public static TileOrientation GetOppositeOrientation(this TileOrientation orientation) {
            switch (orientation) {
                case TileOrientation.Left: return TileOrientation.Right;
                case TileOrientation.Up: return TileOrientation.Down;
                case TileOrientation.Right: return TileOrientation.Left;
                case TileOrientation.Down: return TileOrientation.Up;
                default: return TileOrientation.Right;
            }
        }

        public static TileOrientation GetOrientationFromDegree(int degree) {
            int degMod = (degree % 360);
            switch (degMod) {
                case 0: return TileOrientation.Up;
                case 180: return TileOrientation.Down;
                case 270: return TileOrientation.Left;
                case 90: return TileOrientation.Right;
                default: return TileOrientation.Up;
            }
        }
    }
}