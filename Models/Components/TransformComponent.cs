using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Avalonia;
using Avalonia.Media;
using Nova_Engine.NovaLib.Editor;
using Nova_Engine.Object;

namespace Nova_Engine.Models.Components;

[DataContract]
public class TransformComponent : IEntityComponent, INotifyPropertyChanged
{
    private const string _dllImportPath = "libEngine";
    
    private IntPtr _pointer = IntPtr.Zero;
    private Entity? _entity;

    [DataMember] private float _x, _y, _rot;
    [DataMember] private float _scale = 1;
    [DataMember] private int _width = 30;
    [DataMember] private int _height = 30;
    public float PosX
    {
        get => _x;
        set
        {
            _x = value; 
            UpdateEntityRender();
            OnPropertyChanged();
        }
    }

    public float PosY
    {
        get => _y;
        set
        {
            _y = value; 
            UpdateEntityRender();
            OnPropertyChanged();
        }
    }

    public float Rot
    {
        get => _rot;
        set
        {
            _rot = value; 
            UpdateEntityRender();
            OnPropertyChanged();
        }
    }

    public float Scale
    {
        get => _scale;
        set
        {
            _scale = value; 
            UpdateEntityRender();
            OnPropertyChanged();
        }
    }

    public int Width
    {
        get => _width;
        set
        {
            _width = (int)(value * _scale);
            UpdateEntityRender();
        }
    }
    
    public int Height
    {
        get => _height;
        set
        {
            _height = (int)(value * _scale);
            UpdateEntityRender();
        }
    }
    
    [DllImport(_dllImportPath, CallingConvention = CallingConvention.StdCall)]
    private static extern IntPtr createTransform(float x, float y, float xRot, float yRot, float scale);

    public TransformComponent(float x, float y, float xRot, float yRot, float scale, Entity entity)
    {
        _entity = entity;
        Scale = scale;
        _pointer = createTransform(x, y, xRot, yRot, Scale);
    }

    public void LoadComponent(Entity entity)
    {
        _entity = entity;
        UpdateEntityRender();
        _pointer = createTransform(_x, _y, _rot, 0, _scale);
    }

    private void UpdateEntityRender()
    {
        if (_entity == null) return;
        var renderer = _entity.EntityRenderer;
        renderer.PosX = PosX;
        renderer.PosY = PosY;
        renderer.Width = Scale * Width;
        renderer.Height = Scale * Height;
        
        var mi = Matrix.Identity;
        mi *= Matrix.CreateRotation(_rot * Math.PI / 180);
        renderer.Transform = new MatrixTransform(mi);
        renderer.UpdateGizmo();
    }
    
    public IntPtr GetPointer() => _pointer;
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}