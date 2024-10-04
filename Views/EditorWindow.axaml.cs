using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Nova_Engine.Models.Components;
using Nova_Engine.Object;
using Nova_Engine.ViewModels;

namespace Nova_Engine.Views;

public partial class EditorWindow : Window
{
    private const string _dllImportPath = "libEngine";

    [DllImport(_dllImportPath, CallingConvention = CallingConvention.StdCall)]
    private static extern void run();
    
    [DllImport(_dllImportPath, CallingConvention = CallingConvention.StdCall)]
    private static extern void stop();

    public static Canvas EditorViewport { get; private set; }

    private readonly ZoomBorder? _zoomBorder;
    private string[] _rulerX, _rulerY;
    
    public EditorWindow()
    {
        InitializeComponent();
        _zoomBorder = this.Find<ZoomBorder>("ZoomBorder");
        EntityAddBtn.ButtonClicked += EntityAddBtn_Click;
        
        if (_zoomBorder != null)
            _zoomBorder.ZoomChanged += ZoomBorder_ZoomChanged;
        
        EditorViewport = this.Find<Canvas>("Viewport");
    }

    public EditorViewModel? EditorViewModel => DataContext as EditorViewModel;
    
    public void EntityAddBtn_Click(object? sender, EventArgs e)
    {
        var entity = new Entity("NewEntity");
        entity.AddComponent(new TransformComponent(0, 0, 0, 0, 1, entity));
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

    private void ZoomBorder_ZoomChanged(object? sender, ZoomChangedEventArgs e)
    {
        XAxis.StartPoint = new Point(0 - e.OffsetX /e.ZoomX, 0);
        XAxis.EndPoint = new Point(_zoomBorder.Bounds.Width /e.ZoomX - e.OffsetX /e.ZoomX , 0);
        
        YAxis.StartPoint = new Point(0, 0 - e.OffsetY /e.ZoomY);
        YAxis.EndPoint = new Point(0 , _zoomBorder.Bounds.Height /e.ZoomY - e.OffsetY /e.ZoomY);
        DrawRuler((float)e.ZoomX, e.ZoomY);
    }

    private void DrawRuler(float zoomX, double zoomY)
    {
        _rulerX = new string[20];
        _rulerY = new string[20];
        var space = 0.0f;
        for (int i = 0; i < _rulerX.Length; i++)
        {
            _rulerX[i] = MathF.Floor(space).ToString();
            _rulerY[i] = MathF.Floor(space).ToString();
            space += 100 /zoomX;
        }
        RulerX.ItemsSource = _rulerX;
        RulerY.ItemsSource = _rulerY;
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Image image)
        {
            var presenter = image.FindAncestorOfType<ContentPresenter>();
            if (presenter != null)
            {
                if (Entities.ItemFromContainer(presenter) is Entity entity)
                {
                    EditorViewModel.SelectedEntity = entity;
                }
            }
        }
    }
}