using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Nova_Engine.Object;

[DataContract]
public class Project : INotifyPropertyChanged
{
    [DataMember] private string _name = "NewProject";
    [DataMember] private string _path = "DefaultPath/Nova-Engine";
    [DataMember] public DateTime CreationDate { get; set; }
    [DataMember] public DateTime ModificationDate { get; set; }
    public string Name { get => _name; set { if (_name == value) return; _name = value; OnPropertyChanged(); } }
    public string Path { get => _path; set { if (_path == value) return; _path = value; OnPropertyChanged(); } }
    
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}