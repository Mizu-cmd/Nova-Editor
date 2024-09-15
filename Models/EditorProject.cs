using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Nova_Engine.Util;

namespace Nova_Engine.Object;

[DataContract]
public class EditorProject : INotifyPropertyChanged
{
    [DataMember] private string _name;
    [DataMember] private string _description;
    [DataMember] private string _author;
    [DataMember] private string _version;
    [DataMember] private string _path;
    [DataMember] private string _startScenePath;
    
    public string Name{ get => _name; set { if (_name == value) return; _name = value; OnPropertyChanged(); } }
    public string Description { get => _description; set { if (_description == value) return; _description = value; OnPropertyChanged(); } }
    public string Author { get => _author; set { if (_author == value) return; _author = value; OnPropertyChanged(); } }
    public string Version { get => _version; set { if (_version == value) return; _version = value; OnPropertyChanged(); } }
    public string Path { get => _path; set { if (_path == value) return; _path = value; OnPropertyChanged(); } }
    public string StartScenePath { get => _startScenePath; set { if (_startScenePath == value) return; _startScenePath = value; OnPropertyChanged(); } }

    public EditorProject(string name, string path)
    {
        _name = name;
        _path = path;
    }

    public void SaveProject()
    {
        Serializer.Serialize(this, $"{Path}/{Name}.nova");
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
