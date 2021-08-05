using Godot;
using System;

public class GridPlayer : Node2D {

    private float duration = 10.0f;
    private float time = 0.0f;

    private Vector2 target;

    private int[] directions = {
        // Up
        0,
        0,
        1,
        1,
        2,
        2,
        3,
        3
    };

    private int dirIndex; 
    
    public override void _Ready() {
        dirIndex = 0;
        target = Position;
        target = GetTarget();
    }

    public override void _Process(float delta) {
        time += delta;

        if (time >= 1.0f) {
            time = 1.0f;
        }

        float t = time / duration;

        Position = LinearInterpolate(Position, target, t);
    }

    public void onStateUpdate() {
        dirIndex = (dirIndex + 1) % directions.Length;
        target = GetTarget();
    }

    private Vector2 GetTarget() {
        switch (directions[dirIndex]) {
            case 0: return target + 64f * Vector2.Up;
            case 1: return target + 64f * Vector2.Right;
            case 2: return target + 64f * Vector2.Down;
            case 3: return target + 64f * Vector2.Left;
            default: return target;
        }
    }
        
    private Vector2 LinearInterpolate(Vector2 A, Vector2 B, float t) {
        return (1.0f - t) * A + t * B;
    }
}
