using Godot;

namespace EngineLearning.scripts.mathematics {
    public class Line {
        internal Vector2 P { get; }
        internal Vector2 Dir { get; set; }

        internal float Length { get; }
        
        internal Line(Vector2 position, Vector2 direction, float length) {
            P = position;
            Dir = direction;
            Length = length;
        }

        internal Vector2 Project(Vector2 Q) {
            Vector2 w = Q - P;

            float scalar = (Dir.x * w.x + Dir.y * w.y) / (Dir.x * Dir.x + Dir.y * Dir.y);
            return P + scalar * Dir;
        }

        internal float DistanceTo(Vector2 Q) {
            Vector2 w = Q - P;
            float wDot = w.x * w.x + w.y * w.y;
            float pDot = P.x * P.x + P.y * P.y;
            float wpDot = P.x * w.x + P.y * w.y;

            return Mathf.Sqrt(wDot - (wpDot * wpDot) / pDot);
        }

        internal void ClosestPoints(Line other, out Vector2 point1, out Vector2 point2) {
            Vector2 w0 = P - other.P;
            float a = Dir.x * Dir.x + Dir.y * Dir.y;
            float b = Dir.x * other.Dir.x + Dir.y * other.Dir.y;
            float c = other.Dir.x * other.Dir.x + other.Dir.y * other.Dir.y;
            float d = Dir.x * w0.x + Dir.y * w0.y;
            float e = other.Dir.x * w0.x + other.Dir.y * w0.y;

            float denom = a * c - b * b;

            if (Mathf.Abs(denom) < 1E-4) {
                point1 = P;
                point2 = other.P + (e / c) * other.Dir;
            }
            else {
                point1 = P + ((b*e-c*d)/denom) * Dir;
                point2 = other.P + ((a*e - b*d)/denom) *other.Dir;
            }
        }
    }
}