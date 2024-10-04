using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Nova_Engine.Object;

namespace Nova_Engine.Util.Gizmos;

public class RotationGizmo : Gizmo
{
    private Ellipse? _circle;
    private bool _dragging;
    private Point _startPos;
    private Vector _v;
    private double _startRot;
    public RotationGizmo(Entity entity, GizmoType type) : base(entity, type)
    {
        Draw();
    }

    public sealed override void Draw()
    {
        base.Draw();
        _circle = new Ellipse();
        _circle.Height = Entity.EntityRenderer.Height + 120;
        _circle.Width = Entity.EntityRenderer.Width + 120;
        _circle.Stroke = new SolidColorBrush(Colors.LightBlue);
        _circle.StrokeThickness = 5;
        _circle.Fill = new SolidColorBrush(Colors.Transparent);
        _circle.PointerEntered += PointerEnter;
        _circle.PointerExited += PointerExit;
        _circle.PointerPressed += CircleOnPointerPressed;
        _circle.PointerReleased += CircleOnPointerReleased;
        _circle.PointerMoved += CircleOnPointerMoved;
        Viewport.Children.Add(_circle);
        UpdatePosition();
    }

    private void CircleOnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_dragging == false) return;                
        
        Vector u = CalculateVector(e.GetPosition(Viewport));
        
        double angle = Math.Atan2(u.X, u.Y) - Math.Atan2(_v.X, _v.Y);
        angle = angle * 180 / Math.PI;
        
        Entity.TransformComponent.Rot =  (float)((float) _startRot + angle);
    }

    private void CircleOnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _dragging = false;
    }

    private void CircleOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _startPos = new Point(Renderer.PosX + Renderer.Width / 2.0, -Renderer.PosY - Renderer.Height / 2.0);
        _startRot = Entity.TransformComponent.Rot;
        _dragging = true;
        _v = CalculateVector(e.GetPosition(Viewport));
    }

    private Vector CalculateVector(Point mousePos)
    {
        var centerX = _startPos.X;
        var centerY = _startPos.Y;
        
        return new Vector(mousePos.X - centerX, -(mousePos.Y - centerY));
    }

    public override void UpdatePosition()
    {
        base.UpdatePosition();
        if (_circle == null) return;
        Canvas.SetLeft(_circle, Renderer.PosX + Renderer.Width / 2.0 - _circle.Width / 2.0);
        Canvas.SetTop(_circle, -Renderer.PosY - Renderer.Height / 2.0  - _circle.Height / 2.0);
    }

    public override void Erase()
    {
        base.Erase();
        Viewport.Children.Remove(_circle);
    }
    
    private void PointerExit(object? sender, PointerEventArgs e)
    {
        ((sender as Ellipse)!).Opacity = 1;
    }

    private void PointerEnter(object? sender, PointerEventArgs e)
    {
        ((sender as Ellipse)!).Opacity = 0.5;
    }
}