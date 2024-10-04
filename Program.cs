using System;
using System.Runtime.InteropServices;
using Avalonia;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;

namespace Nova_Engine;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    
    private const string _dllImportPath = "libNova_Engine";
    [DllImport(_dllImportPath, CallingConvention = CallingConvention.StdCall)]
    private static extern void setPathImg(string path);
    
    [DllImport(_dllImportPath, CallingConvention = CallingConvention.StdCall)]
    private static extern void run();
    
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        //setPathImg("C:\\Users\\mitha\\RiderProjects\\Nova-Editor\\Assets\\nova-logo.png");
        //run();
    }
    

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        IconProvider.Current .Register<FontAwesomeIconProvider>();
        
       return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}
