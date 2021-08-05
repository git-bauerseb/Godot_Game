using Godot;

public class Movement : Node2D {

    private Vector2 A;
    private Vector2 B;

    private Vector2[] points =
    {
        new Vector2(50, 100),
        new Vector2(600, 130),
        new Vector2(300, 600),
        new Vector2(50, 100),
    };

    private float[] times;

    private float time;
    private int timeDirection = 1;

    private float duration = 2.5f;

    public override void _Ready() {
        /*
        Vector2 screenSize = GetViewportRect().Size;
        A = new Vector2(screenSize.x * 0.1f, screenSize.y * 0.5f);
        B = new Vector2(screenSize.x - screenSize.x * 0.1f, screenSize.y * 0.6f);
        */

        times = new float[points.Length];

        float totalLength = 0.0f;
        for (int i = 1; i < points.Length; i++) {
            totalLength += points[i].DistanceTo(points[i - 1]);
        }

        times[0] = 0.0f;
        
        for (int i = 1; i < points.Length; i++) {
            times[i] = points[i].DistanceTo(points[i - 1]) / totalLength + times[i-1];
        }
    }

    public override void _Process(float delta) {
        
        if ((time > (1.0f * duration)) || (time < 0.0f)) {
            timeDirection = -timeDirection;
        }

        time += (delta * timeDirection);
        float t = time / duration;

        Position = PiecewiseLinearInterpolation(t, points.Length);
        
    }

    private Vector2 LinearInterpolation(Vector2 S, Vector2 E, float t) {
        return (1 - t) * S + t * E;
    }

    private Vector2 PiecewiseLinearInterpolation(float t, int count) {
        if (t <= times[0]) {
            return points[0];
        }

        if (t >= times[count - 1]) {
            return points[count - 1];
        }

        int i = 0;
        for (; i < count - 1; i++) {
            if (t < times[i + 1]) {
                break;
            }
        }

        float t0 = times[i];
        float t1 = times[i + 1];
        float u = (t - t0) / (t1 - t0);
        
        return (1 - u) * points[i] + u * points[i + 1];
    }
}
