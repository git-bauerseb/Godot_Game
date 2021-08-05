using Godot;
using System;

public class UI : Control {

    private Panel settingsPanel;
    private Tween tween;
    
    public override void _Ready() {
        settingsPanel = GetNode<Panel>("settings_panel");
        settingsPanel.Visible = false;

        tween = GetNode<Tween>("Tween");
    }

    public void onSettingsBtnPressed() {

        Vector2 viewport = GetViewportRect().Size;

        Vector2 settingsPanelStartPos = new Vector2(1.5f * viewport.x, 0.5f * viewport.y);
        Vector2 settingsPanelEndPos = new Vector2(160, 320);
        
        settingsPanel.RectPosition = settingsPanelStartPos;
        settingsPanel.Visible = true;

        tween.InterpolateProperty(settingsPanel, "rect_position", settingsPanelStartPos, settingsPanelEndPos, 0.5f,
            Tween.TransitionType.Cubic, Tween.EaseType.InOut);
        tween.Start();
    }
}
