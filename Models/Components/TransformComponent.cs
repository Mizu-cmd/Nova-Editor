using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Nova_Engine.NovaLib.Editor;

namespace Nova_Engine.Models.Components;

[DataContract]
public class TransformComponent : IEntityComponent
{
    private const string _dllImportPath = @"..\..\..\bin\Debug\net8.0\libNova_Engine.dll";
    
    private IntPtr _pointer = IntPtr.Zero;
    
    [DataMember]
    public Vector2 Position { get; set; }
    [DataMember]
    public Vector2 Rotation { get; set; }
    [DataMember]
    public float Scale { get; set; }

    
    [DllImport(_dllImportPath, CallingConvention = CallingConvention.StdCall)]
    private static extern IntPtr createTransform(float x, float y, float xRot, float yRot, float scale);

    public TransformComponent(float x, float y, float xRot, float yRot, float scale)
    {
        Position = new Vector2(x, y);
        Rotation = new Vector2(xRot, yRot);
        Scale = scale;
        LoadComponent();
    }
    public void LoadComponent() => _pointer = createTransform(Position.X, Position.Y, Rotation.X, Rotation.Y, Scale);
    public IntPtr GetPointer() => _pointer;
}