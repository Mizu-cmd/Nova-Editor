using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Nova_Engine.Util;

namespace Nova_Engine.Object;

[DataContract]
public class Scene : INotifyPropertyChanged
{
    [DataMember]
    public string Name { get; set; }
    
    [DataMember]
    public string Path { get; set; }
    
    [DataMember]
    private ObservableCollection<Entity> _actors;
    public ObservableCollection<Entity> Actors
    {
        get => _actors;
        set
        {
            if (value == _actors) return;
            _actors = value;
            OnPropertyChanged();
        }
    }

    public Scene(string name, string path)
    {
        Name = name;
        Path = path;
        _actors = new ObservableCollection<Entity>();
    }
    
    public void SaveScene()
    {
        Serializer.Serialize(this, GetFullPath());
        Console.WriteLine($"Saved Scene {Name}");
    }

    public void AddActor(Entity entity)
    {
        Actors.Add(entity);
        SaveScene();
    }
    
    public string GetFullPath() => $"{Path}/{Name}.scene";
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}