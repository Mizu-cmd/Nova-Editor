using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using Nova_Engine.Models.Components;
using Nova_Engine.Object;
using Nova_Engine.ViewModels;

namespace Nova_Engine.UserControls;

public partial class EntityBrowser : UserControl
{
    public EntityBrowser()
    {
        InitializeComponent();
    }

    public EditorViewModel? EditorViewModel => DataContext as EditorViewModel;
    
    private void AddChild(object? sender, RoutedEventArgs e)
    {
        Entity? entity = Hierarchy.SelectedItem as Entity;
        var child = new Entity("NewChild");
        child.AddComponent(new TransformComponent(0, 0, 0, 0, 1, entity));
        entity.Children.Add(child);
        Hierarchy.ExpandSubTree((TreeViewItem)Hierarchy.ContainerFromItem(entity));
        Hierarchy.SelectedItem = child;
        EditorViewModel.CurrentScene.SaveScene();
    }

    private void Rename(object? sender, RoutedEventArgs e)
    {
        Entity? entity = Hierarchy.SelectedItem as Entity;
        var container = Hierarchy.ContainerFromItem(entity);
        
        var textBox = container.FindDescendantOfType<TextBox>();
        if (textBox != null)
        {
            Console.WriteLine(entity.Name);
            textBox.IsEnabled = true;
            textBox.Focus();
            textBox.SelectAll();
        }
    }

    private void InputElement_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            var textBox = sender as TextBox;
            Entity? entity = Hierarchy.SelectedItem as Entity;
            entity.Name = textBox.Text;
            textBox.IsEnabled = false;
        }
    }

    private void Hierarchy_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Entity? entity = Hierarchy.SelectedItem as Entity;
        EditorViewModel.SelectedEntity = entity;
    }
}