
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
        public static IntVec2 GetOffsetVec(this Direction direction) {
            switch (direction) {
                case Direction.Down: return new IntVec2(0, 1);
                case Direction.Up: return new IntVec2(0, -1);
                case Direction.Left: return new IntVec2(-1, 0);
                case Direction.Right: return new IntVec2(1, 0);
                default: return new IntVec2(0, 0);
            }
        }
    }
}