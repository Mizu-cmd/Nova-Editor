using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Nova_Engine.Models;

public partial class EntityRenderer : ObservableObject
{
    [ObservableProperty] private Bitmap _bitmap;
    [ObservableProperty] private float _width;
    [ObservableProperty] private float _height;
    [ObservableProperty] private double _posX;
    [ObservableProperty] private float _posY;
    [ObservableProperty] private bool _hasTexture;
    [ObservableProperty] private MatrixTransform _transform;
    [ObservableProperty] private Brush _selectionBrush;
}