using Godot;
using System;

public class TimerExample : Node {

    private Sprite icon;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        icon = GetNode<Sprite>("icon");
    }

    public void timerTimeout() {
        icon.Visible = !icon.Visible;
    }
}
