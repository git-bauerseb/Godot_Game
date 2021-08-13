using System.Runtime.CompilerServices;

namespace EngineLearning.scripts.pushy {
    public struct IntVec2 {
        public int X { get; }
        public int Y { get; }

        public IntVec2(int x, int y) {
            X = x;
            Y = y;
        }

        public static IntVec2 operator +(IntVec2 first, IntVec2 second) {
            return new IntVec2(first.X + second.X, first.Y + second.Y);
        }

        public static bool operator ==(IntVec2 first, IntVec2 second) => first.Equals(second);
        public static bool operator !=(IntVec2 first, IntVec2 second) => !(first == second);

        public override bool Equals(object obj) {
            return obj is IntVec2 other && Equals(other);
        }

        public bool Equals(IntVec2 other) {
            return this.X == other.X && this.Y == other.Y;
        }
    }
}