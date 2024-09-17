using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace Nova_Engine.UserControls.Components;

public partial class Transform : UserControl
{
    
    private bool _isDragging;
    private decimal? _startValue = 0;
    private Point _startPos;
    
    public Transform()
    {
        InitializeComponent();
    }
    
    private void OnMouseDragged(object? sender, PointerEventArgs e)
    {
        if (sender is NumericUpDown s)
            if (_isDragging)
            {
                s.Value = s.Name == "Scale"
                    ? s.Value = _startValue + (decimal?)(e.GetCurrentPoint(this).Position.X - _startPos.X) / 100
                    : s.Value = _startValue + (decimal?)(e.GetCurrentPoint(this).Position.X - _startPos.X);
            }
                
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _isDragging = false;
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _isDragging = true;
        _startPos = e.GetPosition(this);
        if (sender is NumericUpDown s) _startValue = s.Value;
    }
}