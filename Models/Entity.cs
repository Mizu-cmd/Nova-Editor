using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Nova_Engine.Models.Components;
using Nova_Engine.NovaLib.Editor;
using ReactiveUI;

namespace Nova_Engine.Object;

[DataContract]
[KnownType(typeof(Entity))]
[KnownType(typeof(TransformComponent))]
[KnownType(typeof(TextureComponent))]
public class Entity : INotifyPropertyChanged
{
    
    private const string _dllImportPath = "libNova_Engine";
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
        _name = name;
        Id = Guid.NewGuid().ToString();
        _components = new ObservableCollection<IEntityComponent>();
        _children = new ObservableCollection<Entity>();
        LoadEntity();
    }
    
    public void LoadEntity() => _pointer = createEntity();

    public void LoadComponents()
    {
        foreach (var comp in _components)
        {
            comp.LoadComponent();
            addComponent(_pointer, comp.GetPointer());
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}