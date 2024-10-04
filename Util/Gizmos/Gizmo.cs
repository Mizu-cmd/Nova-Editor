using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Nova_Engine.Models;
using Nova_Engine.Models.Components;
using Nova_Engine.Object;
using Nova_Engine.Views;

namespace Nova_Engine.Util.Gizmos;

public abstract class Gizmo
{
    public GizmoType Type;
    protected Entity Entity;
    private Shape _centerPoint;

    public Gizmo(Entity entity, GizmoType type)
    {
        Entity = entity;
        Type = type;
        _centerPoint = new Ellipse();
    }

    protected EntityRenderer Renderer => Entity.EntityRenderer;
    protected TransformComponent Transform => Entity.TransformComponent;
    protected Canvas Viewport => EditorWindow.EditorViewport;
    
    public virtual void Draw()
    {
        _centerPoint.Fill = Brushes.Yellow;
        _centerPoint.Height = 10;
        _centerPoint.Width = 10;
        _centerPoint.ZIndex = 20;
        Viewport.Children.Add(_centerPoint);
        UpdatePosition();
    }

    public virtual void UpdatePosition()
    {
        Canvas.SetLeft(_centerPoint, Transform.PosX + Renderer.Width / 2.0);
        Canvas.SetTop(_centerPoint, -Transform.PosY - Renderer.Height / 2.0);
        Matrix m = Matrix.Identity;
        m *= Matrix.CreateScale(Transform.Scale, Transform.Scale);
        _centerPoint.RenderTransform = new MatrixTransform(m);
    }

    public virtual void Erase()
    {
        EditorWindow.EditorViewport.Children.Remove(_centerPoint);
    }
}