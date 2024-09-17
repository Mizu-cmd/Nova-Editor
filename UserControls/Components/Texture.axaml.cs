using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Nova_Engine.Models.Components;

namespace Nova_Engine.UserControls.Components;

public partial class Texture : UserControl
{
    public Texture()
    {
        InitializeComponent();
        AddHandler(DragDrop.DropEvent, Drop);
        AddHandler(DragDrop.DragOverEvent, DragOver);
    }
    
    private TextureComponent? TextureComponent => DataContext as TextureComponent;
    
    private void Drop(object? sender, DragEventArgs e)
    {
        if (TextureComponent == null) return;
        var path = e.Data.Get(DataFormats.Text) as string;
        
        if (path == null) return;
        
        TextureComponent.TexturePath = path;
        var p = path.Split('/');
        TextureComponent.TextureName = p[p.Length - 1].Split('.')[0];
        TextureComponent.Bitmap = new Bitmap(path);
        Test.Source = TextureComponent.Bitmap;
    }

    private void DragOver(object? sender, DragEventArgs e)
    {
        
    }
}