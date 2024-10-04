using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Nova_Engine.Object;

namespace Nova_Engine.Util.Gizmos;

public class TranslationGizmo : Gizmo
{
    private Image? _arrowRight;
    private Image? _arrowUp;
    private bool _dragging;
    private Point _startPos;
    private float _startValueX;
    private float _startValueY;
    private Bitmap _btmUp;
    private Bitmap _btmRight;
    
    public TranslationGizmo(Entity entity, GizmoType type) : base(entity, type)
    {
        Draw();
    }

    public sealed override void Draw()
    {
        base.Draw();
        _arrowUp = new Image();
        _btmUp = new Bitmap(AssetLoader.Open(new Uri("avares://Nova-Engine/Assets/arrow-up.png")));
        _arrowUp.Source = _btmUp;
        _arrowUp.RenderTransformOrigin = new RelativePoint(0.5, 1 ,RelativeUnit.Relative );
        
        _arrowUp.PointerPressed += ArrowUpOnPointerPressed;
        _arrowUp.PointerReleased += ArrowUpOnPointerReleased;
        _arrowUp.PointerEntered += PointerEnter;
        _arrowUp.PointerExited += PointerExit;
        _arrowUp.PointerMoved += ArrowUpOnPointerMoved;
        Viewport.Children.Add(_arrowUp);
        
        _arrowRight = new Image();
        _btmRight = new Bitmap(AssetLoader.Open(new Uri("avares://Nova-Engine/Assets/arrow-right.png")));
        _arrowRight.Source = _btmRight;
        _arrowRight.RenderTransformOrigin = new RelativePoint(0, 0.5 ,RelativeUnit.Relative );
        _arrowRight.PointerEntered += PointerEnter;
        _arrowRight.PointerExited += PointerExit;
        _arrowRight.PointerPressed += ArrowRightOnPointerPressed;
        _arrowRight.PointerReleased += ArrowRightOnPointerReleased;
        _arrowRight.PointerMoved += ArrowRightOnPointerMoved;
        Viewport.Children.Add(_arrowRight);
        
        UpdatePosition();
    }

    private void ArrowRightOnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_dragging)
        {
            if (Type == GizmoType.Local)
            {
                var mouseDelta = new Point(e.GetPosition(Viewport).X, -e.GetPosition(Viewport).Y)  - _startPos;
                var rotation = (Transform.Rot + 90) * Math.PI / 180;
                
                float cosTheta = (float)Math.Sin(rotation);
                float sinTheta = (float)Math.Cos(rotation);
                
                Vector axisDirection = new Vector(cosTheta, sinTheta);
                var projectionMagnitude = Vector.Dot(mouseDelta, axisDirection);
                Vector projectedMouseDelta = projectionMagnitude * axisDirection;
                
                Transform.PosX = (float)(_startValueX + projectedMouseDelta.X);
                Transform.PosY = (float)(_startValueY +  projectedMouseDelta.Y);
            }else 
                Transform.PosX = _startValueX + (float)(e.GetPosition(Viewport).X - _startPos.X);
        }
    }
    private void ArrowRightOnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
       _dragging = false;
       if (Type == GizmoType.Global)
        Viewport.Cursor = new Cursor(StandardCursorType.Arrow);
    }

    private void ArrowRightOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _dragging = true;
        _startValueX = Transform.PosX;
        _startValueY = Transform.PosY;
        _startPos = new Point(e.GetPosition(Viewport).X, -e.GetPosition(Viewport).Y);
        if (Type == GizmoType.Global)
            Viewport.Cursor = new Cursor(StandardCursorType.SizeWestEast);
    }

    public override void UpdatePosition()
    {
        if (_arrowUp == null || _arrowRight == null) return;
        base.UpdatePosition();
        Canvas.SetLeft(_arrowUp, (Transform.PosX + Renderer.Width / 2.0 + 5) - _btmUp.Size.Width / 2.0);
        Canvas.SetTop(_arrowUp, (-Transform.PosY - Renderer.Height / 2.0 + 3) - _btmUp.Size.Height);
        Canvas.SetLeft(_arrowRight, Transform.PosX + Renderer.Width / 2.0 + 5);
        Canvas.SetTop(_arrowRight, (-Transform.PosY - Renderer.Height / 2.0 + 3) - _btmRight.Size.Height / 2.0);
        
        if (Type == GizmoType.Local)
        {
            Matrix mat = Matrix.Identity;
            mat *= Matrix.CreateRotation(Transform.Rot * Math.PI / 180);
            _arrowUp.RenderTransform = new MatrixTransform(mat);
            _arrowRight.RenderTransform = new MatrixTransform(mat);
        }
        
        Matrix m = Matrix.Identity;
        m *= Matrix.CreateScale(Transform.Scale, Transform.Scale);
        _arrowUp.RenderTransform = new MatrixTransform(m);
        _arrowRight.RenderTransform = new MatrixTransform(m);
    }
    
    private void ArrowUpOnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_dragging)
        {
            if (Type == GizmoType.Local)
            {
                var mouseDelta = new Point(e.GetPosition(Viewport).X, -e.GetPosition(Viewport).Y)  - _startPos;
                var rotation = Transform.Rot * Math.PI / 180;
                
                float cosTheta = (float)Math.Sin(rotation);
                float sinTheta = (float)Math.Cos(rotation);
                
                Vector axisDirection = new Vector(cosTheta, sinTheta);
                var projectionMagnitude = Vector.Dot(mouseDelta, axisDirection);
                Vector projectedMouseDelta = projectionMagnitude * axisDirection;
                
                Transform.PosX = (float)(_startValueX + projectedMouseDelta.X);
                Transform.PosY = (float)(_startValueY +  projectedMouseDelta.Y);
            }else 
                Transform.PosY = (float)(_startValueY + (-e.GetPosition(Viewport).Y - _startPos.Y));
        }
        
    }


    private void PointerExit(object? sender, PointerEventArgs e)
    {
        ((sender as Image)!).Opacity = 1;
    }

    private void PointerEnter(object? sender, PointerEventArgs e)
    {
        ((sender as Image)!).Opacity = 0.5;
    }

    private void ArrowUpOnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _dragging = false;
        Viewport.Cursor = new Cursor(StandardCursorType.Arrow);
    }
    private void ArrowUpOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _dragging = true;
        _startValueX = Entity.TransformComponent.PosX;
        _startValueY = Entity.TransformComponent.PosY;
        _startPos = new Point(e.GetPosition(Viewport).X, -e.GetPosition(Viewport).Y);
        
        if (Type == GizmoType.Global)
            Viewport.Cursor = new Cursor(StandardCursorType.SizeNorthSouth);
    }
    
    public override void Erase()
    {
        base.Erase();
        Viewport.Children.Remove(_arrowRight);
        Viewport.Children.Remove(_arrowUp);
    }
}
