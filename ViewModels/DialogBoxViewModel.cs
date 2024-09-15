using System;
using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nova_Engine.Views.Dialog;

namespace Nova_Engine.ViewModels;

public partial class DialogBoxViewModel : ViewModelBase
{
    
    private DialogBox _dialogBox;
    public string? Title { get; set; }
    [ObservableProperty]
    private string _message;
    [ObservableProperty] private string? _icon;
    public List<Button>? Buttons { get; private set; }

    public DialogBoxViewModel(DialogBox dialogBox)
    {
        _dialogBox = dialogBox;
        Buttons = new List<Button>();
    }

    public void AddButton(Button button)
    {
        Buttons?.Add(button);
    }
}

public class Button
{
    public ICommand ButtonClickCommand { get; set; }
    public string Text { get; private set; }
    private Action _clickEvent;
    public Button(string text, Action clickEvent)
    {
        Text = text;
        _clickEvent = clickEvent;
        ButtonClickCommand = new RelayCommand(_clickEvent);
    }
}