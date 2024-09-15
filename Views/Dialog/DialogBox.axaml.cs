using System;
using Avalonia.Controls;
using Nova_Engine.ViewModels;
using Button = Nova_Engine.ViewModels.Button;

namespace Nova_Engine.Views.Dialog;

public partial class DialogBox : Window
{
    DialogBoxViewModel? _dialogBoxViewModel;
    public DialogBox(string title, string message, string icon)
    {
        DialogBoxViewModel box = new DialogBoxViewModel(this)
        {
            Title = title,
            Message = message,
            Icon = icon,
        };
        _dialogBoxViewModel = box;
        DataContext = box;
        InitializeComponent();

    }
    
    //Preview constructor
    public DialogBox()
    {
        DialogBoxViewModel box = new DialogBoxViewModel(this)
        {
            Title = "Preview",
            Message = "Lorem Ipsum",
            Icon = "/Assets/nova-logo.ico",
        };
        box.AddButton(new Button("OKAY", () => Console.WriteLine("Hello")));
        _dialogBoxViewModel = box;
        DataContext = box;
        InitializeComponent();
    }
    
    public void AddButton(Button button)
    {
        _dialogBoxViewModel?.AddButton(button);
    }
}