using System;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Nova_Engine.Models.Components;
using Nova_Engine.Object;
using Nova_Engine.ViewModels;

namespace Nova_Engine.Views;

public partial class EditorWindow : Window
{
    private const string _dllImportPath = "libNova_Engine";

    [DllImport(_dllImportPath, CallingConvention = CallingConvention.StdCall)]
    private static extern void run();
    
    [DllImport(_dllImportPath, CallingConvention = CallingConvention.StdCall)]
    private static extern void stop();
    
    public EditorWindow()
    {
        InitializeComponent();
        EntityAddBtn.ButtonClicked += EntityAddBtn_Click;
    }

    public EditorViewModel? EditorViewModel =>  DataContext as EditorViewModel;
    
    public void EntityAddBtn_Click(object? sender, EventArgs e)
    {
        var entity = new Entity("NewEntity");
        entity.AddComponent(new TransformComponent(0, 0, 0, 0, 1));
        entity.AddComponent(new TextureComponent()
        {
            TextureName = "NewTexture",
        });
        EditorViewModel?.CurrentScene.AddActor(entity);
    }
    
    private void StartBtn_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        run();
    }
    
    private void StopBtn_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        stop();
    }


    private void SaveMenu(object? sender, RoutedEventArgs e)
    {
        EditorViewModel.CurrentScene.SaveScene();
    }
}