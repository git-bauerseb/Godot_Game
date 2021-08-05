using Godot;
using System;

public class DistanceFunction : Control {
    private bool updateShader = false;

    public override void _Process(float delta) {
        Vector2 mousePos = GetViewport().GetMousePosition();

        mousePos.x /= GetViewport().Size.x;
        mousePos.y /= GetViewport().Size.y;

        (Material as ShaderMaterial).SetShaderParam("mouse", mousePos);
    }

    public void onApplyBtnPressed() {
        File shaderFile = new File();
        shaderFile.Open("res://shader/distance_fct.shader", File.ModeFlags.Read);

        string shaderCode = shaderFile.GetAsText();

        (Material as ShaderMaterial).Shader.Code = shaderCode;
        
        GD.Print("Updated Shader");
    }
}