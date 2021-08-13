
using EngineLearning.scripts.grid_game.misc;

namespace EngineLearning.scripts.pushy {
    public enum Direction {
        None,
        Right,
        Left,
        Up,
        Down
    }
    
    
    public static class DirectionExtension {
        public static Vector2Int GetOffsetVec(this Direction direction) {
            switch (direction) {
                case Direction.Down: return new Vector2Int(0, 1);
                case Direction.Up: return new Vector2Int(0, -1);
                case Direction.Left: return new Vector2Int(-1, 0);
                case Direction.Right: return new Vector2Int(1, 0);
                default: return new Vector2Int(0, 0);
            }
        }
    }
}