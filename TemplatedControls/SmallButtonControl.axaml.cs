using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace Nova_Engine.TemplatedControls;

public class SmallButtonControl : TemplatedControl
{
    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<SmallIconButtonControl, string>(
        nameof(Text), "Lorem");
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    
    public EventHandler ButtonClicked;
    private Button _button;
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var button = e.NameScope.Find<Button>("PART_Button");
        if (button != null)
        {
            button.Click += (sender, args) => ButtonClicked?.Invoke(this, args);
        }
    }
}