using Godot;
using System;

public class ViewportShader : Node2D {
    public override void _Ready() {
        (Material as ShaderMaterial).SetShaderParam("ViewportTexture", GetViewport().GetTexture());
    }
}
