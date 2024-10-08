using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Nova_Engine.Models;
using Nova_Engine.Models.Components;
using Nova_Engine.NovaLib.Editor;

namespace Nova_Engine.Object;

[DataContract]
[KnownType(typeof(Entity))]
[KnownType(typeof(TransformComponent))]
[KnownType(typeof(TextureComponent))]
public class Entity : INotifyPropertyChanged
{
    
    private const string _dllImportPath = "libEngine";
    private IntPtr _pointer = IntPtr.Zero;

    [DataMember] private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (_name == value) return;
            _name = value;
            OnPropertyChanged();
        }
    }
    public string Id { get; private set; }
    
    public EntityRenderer EntityRenderer {get; private set;}

    [DataMember]
    private ObservableCollection<IEntityComponent> _components;
    public ObservableCollection<IEntityComponent> Components
    {
        get => _components;
        private set
        {
            if (_components == value) return;
            _components = value;
            OnPropertyChanged();
        }
    }
    
    [DataMember]
    private ObservableCollection<Entity> _children;
    public ObservableCollection<Entity> Children
    {
        get => _children;
        set
        {
            if (_children == value) return;
            _children = value;
            OnPropertyChanged();
        }
    }

    public TransformComponent TransformComponent => _components.OfType<TransformComponent>().First();
    public List<TextureComponent> TextureComponents => _components.OfType<TextureComponent>().ToList();

    [DllImport(_dllImportPath, CallingConvention = CallingConvention.StdCall)]
    private static extern IntPtr createEntity();
    
    [DllImport(_dllImportPath, CallingConvention = CallingConvention.StdCall)]
    private static extern IntPtr addComponent(IntPtr entity, IntPtr component);

    public void AddComponent(IEntityComponent component)
    {
        addComponent(_pointer, component.GetPointer());
        _components.Add(component);
    }
    
    public Entity(string name)
    {
        EntityRenderer = new EntityRenderer(this);
        _name = name;
        Id = Guid.NewGuid().ToString();
        _components = new ObservableCollection<IEntityComponent>();
        _children = new ObservableCollection<Entity>();
        LoadEntity();
    }
    
    public void LoadEntity() => _pointer = createEntity();

    public void LoadComponents()
    {
        EntityRenderer = new EntityRenderer(this);
        TextureComponents.ForEach((component => component.LoadComponent(this)));
        foreach (var comp in _components)
        {
            if (!(comp is TextureComponent)) comp.LoadComponent(this);
            addComponent(_pointer, comp.GetPointer());
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}