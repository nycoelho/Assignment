using System.Windows;
using System.Windows.Input;
using Assignment.Application.Common.Exceptions;
using Assignment.Application.TodoLists.Commands.CreateTodoList;
using Caliburn.Micro;
using MediatR;

namespace Assignment.UI;
public class TodoListViewModel : Screen
{
    private readonly ISender _sender;

    private string _title;
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            NotifyOfPropertyChange(() => Title);
        }
    }

    public ICommand SaveCommand { get; }
    public ICommand CloseCommand { get; }

    public TodoListViewModel(ISender sender)
    {
        _sender = sender;

        SaveCommand = new RelayCommand(SaveExecute);
        CloseCommand = new RelayCommand(CloseExecute);
    }

    private async void SaveExecute(object parameter)
    {
        try
        {
            await _sender.Send(new CreateTodoListCommand(Title));
        }
        catch (ValidationException vex)
        {
            foreach (string key in vex.Errors.Keys)
            {
                MessageBox.Show(string.Join(", ", vex.Errors[key]), $"Invalid Todo List {key}!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        await TryCloseAsync(true);
    }

    private async void CloseExecute(object parameter)
    {
        await TryCloseAsync(false);
    }
}
