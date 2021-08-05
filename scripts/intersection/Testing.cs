using Godot;
using System;
using EngineLearning.scripts.mathematics;

public class Testing : Node2D {

    /*
    private Line line;
    private Vector2 point;
    private int frame;


    private Label distanceLabel;
    
    public override void _Ready() {

        distanceLabel = GetNode<Label>("../Label");
        
        line = new Line(
            new Vector2(50f, 50f),
            Vector2.Right + Vector2.Down, 100f
            );
    }

    public override void _Process(float delta) {
        frame = (frame++) % 2;
        point = GetViewport().GetMousePosition();

        if ((frame & 0x1) == 0) {
            Update();
        }

        distanceLabel.Text = $"{line.DistanceTo(point)}";
    }

    public override void _Draw() {
        DrawLine(line.P, line.P + line.Length * line.Dir, Colors.Blue, 2f);
        DrawCircle(line.P, 5f, Colors.Red);
        
        DrawCircle(point, 5f, Colors.Yellow);
        DrawCircle(line.Project(point), 5f, Colors.Green);
    }
    */

    private Line line1;
    private Line line2;

    private int frame = 0;

    private float angle;

    public override void _Ready() {
        line1 = new Line(new Vector2(400.0f, 267.0f), Vector2.Right, 100f);
        line2 = new Line(new Vector2(600.0f, 512.0f), Vector2.Left + 0.5f * Vector2.Up, 75f);
    }

    public override void _Process(float delta) {
        frame = (frame+1) % 2;

        // Update very third frame
        if (frame == 0) {
            angle += 45f * delta;

            if (angle >= 360.0f) {
                angle = 0.0f;
            }
        
            // Update line directions
            line1.Dir = Mathf.Cos(Mathf.Deg2Rad(angle)) * Vector2.Right + Mathf.Sin(Mathf.Deg2Rad(angle)) * Vector2.Up;
            
            line2.Dir = Mathf.Cos(Mathf.Deg2Rad(angle + 75f)) * Vector2.Right + Mathf.Sin(Mathf.Deg2Rad(angle + 75f)) * Vector2.Up ;

            Update();
        }
    }

    public override void _Draw() {
        DrawLine(line1);
        DrawLine(line2);

        Vector2 i1, i2;
        
        line1.ClosestPoints(line2, out i1, out i2);
        
        DrawCircle(i1, 5f, Colors.Red);
        DrawCircle(i2, 5f, Colors.Red);
    }

    private void DrawLine(Line line) {
        DrawLine(line.P, line.Length * line.Dir + line.P, Colors.Blue, 2f);
    }
}
