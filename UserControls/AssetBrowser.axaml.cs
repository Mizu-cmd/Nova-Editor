using Avalonia.Controls;
using Nova_Engine.ViewModels;

namespace Nova_Engine.UserControls;

public partial class AssetBrowser : UserControl
{
    public AssetBrowser()
    {
        DataContext = new AssetBrowserViewModel();
        InitializeComponent();
    }
}