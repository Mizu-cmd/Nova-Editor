using Avalonia;
using Avalonia.Media;

namespace Nova_Engine.TemplatedControls;

public class SmallIconButtonControl : SmallButtonControl
{

    public static readonly StyledProperty<string> IconProperty = AvaloniaProperty.Register<SmallIconButtonControl, string>(
        nameof(Icon), "fa-solid fa-plus");

    public string Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<IBrush> IconColorProperty = AvaloniaProperty.Register<SmallIconButtonControl, IBrush>(
        nameof(IconColor), Brushes.Black);

    public IBrush IconColor
    {
        get => GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }
}