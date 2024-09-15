using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Nova_Engine.Object;
using Nova_Engine.Util;
using Nova_Engine.ViewModels;
using Nova_Engine.Views.Dialog;
using Button = Nova_Engine.ViewModels.Button;

namespace Nova_Engine.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ButtonCreate.ButtonClicked += CreateBtn_OnClick;
        ButtonRename.ButtonClicked += RenameBtn_OnClick;
        ButtonDelete.ButtonClicked += RemoveBtn_OnClick;
        ButtonImport.ButtonClicked += ImportBtn_OnClick;
        ButtonLaunch.ButtonClicked += LaunchBtn_OnClick;
    }

    public void LaunchBtn_OnClick(object? sender, EventArgs e)
    {
        if (ProjectList.SelectedItems != null && ProjectList.SelectedItems.Count == 0 ) return;

        var evm = new EditorViewModel();
        var project = ProjectList.Selection.SelectedItem as Project;
        evm.Project = Serializer.Deserialize<EditorProject>($"{project.Path}/{project.Name}.nova");
        evm.CurrentScene = Serializer.Deserialize<Scene>(evm.Project.StartScenePath);
        foreach (var currentSceneActor in evm.CurrentScene.Actors)
        {
            currentSceneActor.LoadEntity();
            currentSceneActor.LoadComponents();
        }
        EditorWindow editorWindow = new EditorWindow()
        {
            DataContext = evm
        };
        if (editorWindow.AssetBrowser.DataContext is AssetBrowserViewModel browser)
        {
            browser.DirPath = evm.Project.Path;
            browser.LoadDirectories();
        }
        editorWindow.Show();
        Console.WriteLine($"opened {project.Name}");
        Close();
    }

    private async void ImportBtn_OnClick(object? sender, EventArgs e)
    {
        var topLevel = GetTopLevel(this);
        var dir = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            AllowMultiple = false,
            Title = "Import Project"
        });

        if (dir.Count > 0)
        {
            MainWindowViewModel? vm = DataContext as MainWindowViewModel;
            var path = dir[0].Path;
            var f = Directory.GetFiles(path.AbsolutePath, "*.nova");
            if (f.Length == 1)
            {
                var project = Serializer.Deserialize<EditorProject>(f[0]);
                vm.AddProject(project.Name, project.Path);
                Console.WriteLine("Imported Project " + project.Name);
            }
            else
            {
                var dialog = new DialogBox("Error", "No Project found in this directory", "Assets/nova-logo.ico");
                dialog.AddButton(new Button("Okay", () => dialog.Close()));
                dialog.Show();
            }
        }
    }
    
    private void CreateBtn_OnClick(object? sender, EventArgs e)
    {
        var createProjectWindow = new CreateProjectWindow((MainWindowViewModel) DataContext);
        createProjectWindow.ShowDialog(this);
    }

    private void RemoveBtn_OnClick(object? sender, EventArgs e)
    {
        var p = ProjectList.SelectedItem as Project;
        if (p == null) return;
        ((MainWindowViewModel) DataContext).RemoveProject(p);

        DialogBox box = new DialogBox("Info",  "Project has been removed from the list. You can reimport it back using the import button", "/Assets/nova-logo.ico");
        box.AddButton(new Button("Okay", () => box.Close()));
        box.ShowDialog(this);
    }

    private void RenameBtn_OnClick(object? sender, EventArgs e)
    {
        var p = ProjectList.SelectedItem as Project;
        if (p == null) return;
        ((MainWindowViewModel) DataContext).RenameProject(p, "Test");
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender != null && DataContext != null)
            ((MainWindowViewModel) DataContext).Sort_Changed(Sort.SelectedItem);
    }

    //Filter project
    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender != null && DataContext != null)
        {
            MainWindowViewModel? vm = DataContext as MainWindowViewModel;
            if (vm == null) return;
            if (FilterTextBox.Text.Length == 0)
            {
                ProjectList.ItemsSource = vm.Projects;
                return;
            }
            var display = new List<Project>();
            foreach (Project p in vm.Projects)
            {
                if (p.Name.ToLower().StartsWith(FilterTextBox.Text.ToLower()))
                {
                    display.Add(p);
                }
            }
            ProjectList.ItemsSource = display;
        }
    }
}