using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Nova_Engine.Models.AssetBrowser;

public class DirectoryNode
{
    public string Name { get; set; }
    public string Icon { get; set; }
    public ObservableCollection<DirectoryNode> Children { get; set; }

    public DirectoryNode(string path)
    {
        Name = Path.GetFileName(path).Split(".")[0];
        Children = new ObservableCollection<DirectoryNode>();
        SetIcon(path);
        LoadSubDirectories(path);
    }

    private void SetIcon(string path)
    {
        if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            Icon = "fa-solid fa-folder";
        else if (path.EndsWith(".scene"))
            Icon = "fa-solid fa-clapperboard";
        else if (path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".jpeg"))
            Icon = "fa-solid fa-image";
        
    }
    
    private void LoadSubDirectories(string path)
    {
        try
        {
            foreach (var directory in Directory.GetFileSystemEntries(path))
                Children.Add(new DirectoryNode(directory));
        }
        catch
        {
            // Handle exceptions (e.g., access denied) if needed
        }
    }
}