using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Nova_Engine.Object;
using Nova_Engine.Util;

namespace Nova_Engine.ViewModels;

public partial class EditorViewModel : ViewModelBase
{
    public EditorProject Project { get; set; } = null!;

    [ObservableProperty]
    private Entity? _selectedEntity;
    
    [ObservableProperty]
    private Scene _currentScene = null!;
    
    public Scene LoadScene(string path)
    {
        Console.WriteLine($"Loaded Scene {path}");
        return Serializer.Deserialize<Scene>(path);
    }

    public void ChangeScene(Scene scene)
    {
        CurrentScene = scene;
    }
}