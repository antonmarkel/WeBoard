using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Tools.Base;

public class MarkerTool : ToolBase
{
    private readonly List<CircleShape> _circles = new();
    private bool _isDrawing = false;

    public override void OnMousePressed(Vector2f worldPos)
    {
        _isDrawing = true;
        AddCircle(worldPos);
    }

    public override void OnMouseReleased(Vector2f worldPos)
    {
        _isDrawing = false;
    }

    public override void OnMouseMoved(Vector2f worldPos)
    {
        if (_isDrawing)
            AddCircle(worldPos);
    }

    private void AddCircle(Vector2f pos)
    {
        var circle = new CircleShape(BrushSize / 2f)
        {
            FillColor = BrushColor,
            Position = pos - new Vector2f(BrushSize / 2f, BrushSize / 2f),
            OutlineThickness = BrushSize / 3f,
            OutlineColor = new Color(BrushColor.R, BrushColor.G, BrushColor.B, (byte)(BrushColor.A / 3))
        };
        
        _circles.Add(circle);
    }

    public override void Draw(RenderTarget target, RenderStates states)
    {
        foreach (var c in _circles)
        {
            target.Draw(c, states);
        }
    }
}