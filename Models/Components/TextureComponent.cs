using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Avalonia.Media.Imaging;
using Nova_Engine.NovaLib.Editor;

namespace Nova_Engine.Models.Components;

[DataContract]
public class TextureComponent : IEntityComponent, INotifyPropertyChanged
{
    private IntPtr _pointer = IntPtr.Zero;
    private const string _dllImportPath = "libNova_Editor.dll";
    
    [DllImport(_dllImportPath, CallingConvention = CallingConvention.StdCall)]
    private static extern IntPtr createTexture();
    
    private Bitmap _bitmap;

    public Bitmap Bitmap
    {
        get => _bitmap;
        set
        {
            _bitmap = value;
            OnPropertyChanged();
        }
    }

    [DataMember]
    public string TextureName { get; set; }
    [DataMember]
    public string TexturePath { get; set; }

    [DataMember] private string _fullPath = "";
    public string FullPath
    {
        get => _fullPath;
        set
        {
            Bitmap = new Bitmap(value);
            _fullPath = value;
            OnPropertyChanged();
        }
    }
    public IntPtr GetPointer() => _pointer;
    public void LoadComponent() {}
    
    
    public void LoadTextureToEngine()
    {
        Bitmap btm = new Bitmap(FullPath);
        Console.WriteLine(btm);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}