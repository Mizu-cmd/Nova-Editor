using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using DynamicData.Kernel;
using Nova_Engine.Object;
using Nova_Engine.Util;

namespace Nova_Engine.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Version => "Nova-Engine V"+Assembly.GetExecutingAssembly().GetName().Version;
    
    private readonly string _projectsPath = "projects.xml";

    [ObservableProperty]
    private ObservableCollection<Project> _projects;

    public MainWindowViewModel()
    {
        _projects = new ObservableCollection<Project>();
        LoadProjects();
    }

    public void CreateProject(string name, string path)
    {
        Directory.CreateDirectory(path);
        Directory.CreateDirectory(path + "/Assets");
        Directory.CreateDirectory(path + "/Assets/Scenes");
        var scene = new Scene("DefaultScene", path + "/Assets/Scenes");
        scene.SaveScene();
        var project = new EditorProject(name, path);
        project.StartScenePath = scene.GetFullPath();
        project.SaveProject();
        AddProject(name, path);
    }
    
    public void AddProject(string name, string path)
    {
        var newProject = new Project
        {
            Name = name,
            Path = path,
            CreationDate = DateTime.Now
        };
        Projects.Insert(0, newProject);
        SaveProjects();
    }
    public void RemoveProject(Project project)
    {
        Projects.Remove(project);
        SaveProjects();
    }
    
    public void RenameProject(Project project, string newName)
    {
        project.Name = newName;
        SaveProjects();
    }
    
    public void SaveProjects()
    {
        Serializer.Serialize(Projects, _projectsPath);
        Debug.WriteLine("Project Saved");
    }
    
    public void LoadProjects()
    {
        if (!File.Exists(_projectsPath)) return;
        Projects = new ObservableCollection<Project>(Serializer.Deserialize<List<Project>>(_projectsPath));
        Debug.WriteLine("Project Loaded");
    }

    public void Sort_Changed(object? selectedItem)
    {
        ComboBoxItem? cmbItem = selectedItem as ComboBoxItem;
        switch (cmbItem.Name)
        {
            case "Alphabetical":
                Projects = new ObservableCollection<Project>(Projects.OrderBy(p => p.Name).ToList());
                break;
            case "CreationDate":
                Projects = new ObservableCollection<Project>(Projects.OrderByDescending(p => p.CreationDate).ToList());
                break;
            case "LastEdited":
                Projects = new ObservableCollection<Project>(Projects.OrderByDescending(p => p.ModificationDate).ToList());
                break;
        }
    }
}
