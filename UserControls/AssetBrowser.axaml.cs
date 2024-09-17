using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using Nova_Engine.Models.AssetBrowser;
using Nova_Engine.ViewModels;

namespace Nova_Engine.UserControls;

public partial class AssetBrowser : UserControl
{
    public AssetBrowser()
    {
        DataContext = new AssetBrowserViewModel();
        InitializeComponent(); ;
    }

    private async void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        
        if (sender is Control control)
        {
            var point = e.GetPosition(AssetBrowserView);
            var hitTestResult = AssetBrowserView.InputHitTest(point);
            var treeViewItem = (hitTestResult as Control)?.GetLogicalAncestors().OfType<TreeViewItem>().FirstOrDefault();
            if (treeViewItem != null)
            {
                var dataContext = treeViewItem.DataContext as DirectoryNode;
                if (dataContext != null)
                {
                if (dataContext.IsDirectory) return;
                    var dataObject = new DataObject();
                    dataObject.Set(DataFormats.Text, dataContext.FullPath);
                    var dragResult = await DragDrop.DoDragDrop(e, dataObject, DragDropEffects.Move);
                }
            }
        }
        
    }
}