using Godot;

namespace EngineLearning.scripts {
    public class HermiteSegment {
        public Vector2 P0 { get; set;  }
        public Vector2 P0Der { get; set; }
        public Vector2 P1 { get; set; }
        public Vector2 P1Der { get; set; }

        private Vector2[] points;

        public HermiteSegment(Vector2 p0, Vector2 p0Der,
            Vector2 p1, Vector2 p1Der) {
            P0 = p0;
            P1 = p1;
            P0Der = p0Der;
            P1Der = p1Der;
        }

        public Vector2[] getPoints(int num) {

            float step = 1.0f / num;
            float t = 0.0f;
            
            if (points == null) {
                points = new Vector2[num];
            }
            
            for (int i = 0; i < num-1; i++) {
                points[i] = calculatePoint(t);
                t += step;
            }

            points[num - 1] = P1;

            return points;
        }

        private Vector2 calculatePoint(float t) {
            float tSq = t * t;
            float tCub = tSq * t;
            
            return (2 * tCub - 3 * tSq + 1) * P0
                   + (-2 * tCub + 3 * tSq) * P1
                   + (tCub - 2 * tSq + t) * P0Der
                   + (tCub - tSq) * P1Der;
        }
    }
}