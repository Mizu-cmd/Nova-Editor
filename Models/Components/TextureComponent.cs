using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Avalonia.Media.Imaging;
using Nova_Engine.NovaLib.Editor;
using Nova_Engine.Object;

namespace Nova_Engine.Models.Components;

[DataContract]
public class TextureComponent : IEntityComponent, INotifyPropertyChanged
{
    private IntPtr _pointer = IntPtr.Zero;
    private const string _dllImportPath = "libEngine";
    private Bitmap _bitmap;
    private Entity _entity;
    
    [DataMember] public string? TexturePath { get; set; }

    public Bitmap Bitmap
    {
        get => _bitmap;
        set
        {
            _bitmap = value;
            _entity.EntityRenderer.Bitmap = _bitmap;
            _entity.TransformComponent.Width = _bitmap.PixelSize.Width;
            _entity.TransformComponent.Height = _bitmap.PixelSize.Height;
            OnPropertyChanged();
        }
    }
    
    [DataMember]
    private string _textureName;

    public string TextureName
    {
        get => _textureName; 
        set
        {
            _textureName = value;
            OnPropertyChanged();
        }
    }
    
    public IntPtr GetPointer() => _pointer;

    public void LoadComponent(Entity entity)
    {
        _entity = entity;
        if (TexturePath != null)
        {
            Bitmap = new Bitmap(TexturePath);
            _pointer = createTexture(TexturePath);
        }
    }
    
    [DllImport(_dllImportPath, CallingConvention = CallingConvention.StdCall)]
    private static extern IntPtr createTexture(string path);
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}