using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Nova_Engine.Object;
using Nova_Engine.Util.Gizmos;

namespace Nova_Engine.Models;

public partial class EntityRenderer(Entity entity) : ObservableObject
{
    [ObservableProperty] private Bitmap? _bitmap;
    [ObservableProperty] private float _width;
    [ObservableProperty] private float _height;
    [ObservableProperty] private double _posX;
    [ObservableProperty] private float _posY;
    [ObservableProperty] private bool _hasTexture;
    [ObservableProperty] private MatrixTransform _transform = null!;
    [ObservableProperty] private Brush _selectionBrush = new SolidColorBrush(Colors.DarkBlue, 0.4f);

    private Gizmo? _gizmo;

    public void UpdateGizmo()
    {
        if (_gizmo != null) _gizmo.UpdatePosition();
    }
    
    public void DeselectEntity()
    {
        SelectionBrush = new SolidColorBrush(Colors.Transparent);
        if (_gizmo != null) _gizmo.Erase();
    }
    
    public void SelectEntity()
    {
        SelectionBrush =  new SolidColorBrush(Colors.DarkBlue, 0.4f);
       _gizmo = new TranslationGizmo(entity, GizmoType.Global);
    }
}