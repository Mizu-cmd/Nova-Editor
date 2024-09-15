using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Nova_Engine.Object;
using Nova_Engine.ViewModels;

namespace Nova_Engine.Views;

public partial class CreateProjectWindow : Window
{
    
    public string DefaultPath => @$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/")}/NovaProject/"+ProjectName.Text;
    public CreateProjectWindow(MainWindowViewModel dataContext)
    {
        InitializeComponent();
        DirPath.Text = DefaultPath;
        DataContext = dataContext;
        Templates.ItemsSource = new string[] { "Test", "Test 2", "Test 3", "Test 4", "Test 5"};
    }
    
    //Preview Constructor
    public CreateProjectWindow()
    {
        InitializeComponent();
        Templates.ItemsSource = new string[] { "Test", "Test 2", "Test 3", "Test 4", "Test 5"};
    }

    private void CancelBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void CreateBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel mainWindowViewModel)
        {
            if (ProjectName.Text != "" && DirPath.Text != null)
            {
                try
                {
                    Path.GetFullPath(DirPath.Text);
                    mainWindowViewModel.CreateProject(ProjectName.Text, DirPath.Text);
                    Close();
                }
                catch (Exception exception)
                {
                    return;
                }
            }
        }
    }    
    
    private async void ChooseDirBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var dir = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            AllowMultiple = false,
            Title = "Choose Directory",
        });

        if (dir.Count > 0)
        {
            DirPath.Text = dir[0].Path.AbsolutePath+"/"+ProjectName.Text;
        }
    }

    private void ProjectName_OnTextChanging(object? sender, TextChangingEventArgs e)
    {
        if (DirPath == null) return;
        var part = DirPath.Text?.Split("/");
        var path = "";
        for (int i = 0; i < part.Length - 1; i++)
        {
            path += part[i] + "/";
        }
        DirPath.Text = path + ProjectName.Text;
    }
}