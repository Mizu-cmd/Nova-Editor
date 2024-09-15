using System;
using System.Collections.ObjectModel;
using System.IO;
using Nova_Engine.Models.AssetBrowser;

namespace Nova_Engine.ViewModels;

public class AssetBrowserViewModel : ViewModelBase
{
    public ObservableCollection<DirectoryNode> Directories { get; set; }
    
    public string DirPath { get; set; }
    public AssetBrowserViewModel()
    {
        Directories = new ObservableCollection<DirectoryNode>();
    }

    public void LoadDirectories()
    {
        foreach (var e in Directory.GetFileSystemEntries(DirPath))
        {
            Directories.Add(new DirectoryNode(e));
        }
    }
}