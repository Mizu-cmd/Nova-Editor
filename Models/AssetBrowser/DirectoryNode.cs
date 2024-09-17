using System.Collections.ObjectModel;
using System.IO;

namespace Nova_Engine.Models.AssetBrowser;

public class DirectoryNode
{
    public string Name { get; set; }
    public string Icon { get; set; }
    public string FullPath { get; set; }
    public bool IsDirectory { get; set; }
    public ObservableCollection<DirectoryNode> Children { get; set; }

    public DirectoryNode(string path)
    {
        FullPath = path.Replace('\\', '/');
        Name = Path.GetFileName(FullPath).Split(".")[0];
        IsDirectory = File.GetAttributes(FullPath).HasFlag(FileAttributes.Directory);
        Children = new ObservableCollection<DirectoryNode>();
        SetIcon(FullPath);
        LoadSubDirectories(FullPath);
    }

    private void SetIcon(string path)
    {
        if (IsDirectory)
            Icon = "fa-solid fa-folder";
        else if (path.EndsWith(".scene"))
            Icon = "fa-solid fa-clapperboard";
        else if (path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".jpeg"))
            Icon = "fa-solid fa-image";
        
    }
    
    private void LoadSubDirectories(string path)
    {
        if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
        {
            foreach (var directory in Directory.GetFileSystemEntries(path))
                Children.Add(new DirectoryNode(directory));
        }
    }
}