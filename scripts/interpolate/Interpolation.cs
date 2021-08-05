using Godot;
using System;
using EngineLearning.scripts;

public class Interpolation : Node2D {
    private HermiteSegment segment;

    private Vector2[] points;

    private Vector2 p1DerStart = new Vector2(170, 290);
    private Vector2 p1DerEnd = new Vector2(700, 130 );

    private float time;
    private int timeDirection = 1;

    private float duration = 2.5f;

    private int pointToModify = 0;
    
    public override void _Ready() {
        segment = new HermiteSegment(
            new Vector2(50, 50),
            new Vector2(65, 100),
            new Vector2(680, 60),
            new Vector2(170, 290)
            );

        points = segment.getPoints(16);
    }

    public override void _Draw() {
        
        DrawPolyline(points, Colors.Red, 3f);
        DrawCircle(segment.P0, 3f, Colors.Blue);
        DrawCircle(segment.P1, 3f, Colors.Blue);
        DrawCircle(segment.P0Der, 3f, Colors.Green);
        
        DrawCircle(segment.P1Der, 3f, Colors.Green);
        
        DrawLine(segment.P0, segment.P0 + (segment.P0Der - segment.P0), Colors.Yellow, 2f);
        DrawLine(segment.P1, segment.P1 + (segment.P1Der - segment.P1), Colors.Yellow, 2f);
    }

    public override void _Process(float delta) {
        /*
        if ((time > (1.0f * duration)) || (time < 0.0f)) {
            timeDirection = -timeDirection;
        }

        time += (delta * timeDirection);
        float t = time / duration;

        segment.P1Der = LinearInterpolation(p1DerStart, p1DerEnd, t);
        */
        var mousePos = GetViewport().GetMousePosition();

        switch (pointToModify) {
            case 0:
                segment.P0 = mousePos;
                break;
            case 1:
                segment.P0Der = mousePos;
                break;
            case 2:
                segment.P1 = mousePos;
                break;
            case 3:
                segment.P1Der = mousePos;
                break;
            default:
                break;
        }

        points = segment.getPoints(16);

        Update();
    }

    private Vector2 LinearInterpolation(Vector2 S, Vector2 E, float t) {
        return (1 - t) * S + t * E;
    }

    
    public override void _Input(InputEvent ev) {
        if (ev is InputEventKey evKey) {
            if (evKey.Pressed && (evKey.Scancode == (int) KeyList.I)) {
                pointToModify = (pointToModify+1) % 4;
                GD.Print($"Point to modify is: {pointToModify}");
            }
        }
        
        /*
        if (ev is InputEventMouseButton evMBtn) {
            if (ev.IsAction()) {
                switch (pointToModify) {
                    case 0: segment.P0 = evMBtn.Position;
                        break;
                    case 1: segment.P0Der = evMBtn.Position;
                        break;
                    case 2: segment.P1 = evMBtn.Position;
                        break;
                    case 3: segment.P1Der = evMBtn.Position;
                        break;
                    default:
                        break;
                }
            }
        }
        */
    }
}
