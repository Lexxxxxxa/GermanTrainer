using System.Windows.Input;

namespace GermanTrainer.ViewModels.Base;

/// <summary>
/// Non-reentrant async command; exceptions are routed to the supplied error handler
/// </summary>
public sealed class AsyncRelayCommand(Func<Task> execute, Action<Exception> error, Func<bool>? canExecute = null) : ICommand
{
    private bool _running;
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
    public bool CanExecute(object? parameter) => !_running && (canExecute?.Invoke() ?? true);
    public async void Execute(object? parameter)
    {
        if (!CanExecute(parameter)) return;
        _running = true; CommandManager.InvalidateRequerySuggested();
        try { await execute(); }
        catch (Exception ex) { error(ex); }
        finally { _running = false; CommandManager.InvalidateRequerySuggested(); }
    }
}
