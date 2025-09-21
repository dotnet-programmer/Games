using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Snake.WpfApp.Commands;

internal class RelayCommandAsync(Func<object?, Task> execute, Predicate<object?>? canExecute = null) : ICommand
{
	private readonly Func<object?, Task> _execute = execute ?? throw new ArgumentNullException(nameof(execute));
	private readonly Predicate<object?>? _canExecute = canExecute;

	private long _isExecuting;

	// Parameterless constructor
	public RelayCommandAsync(Func<Task> execute, Func<bool>? canExecute = null) : this(
		execute != null ? _ => execute() : throw new ArgumentNullException(nameof(execute)),
		canExecute != null ? _ => canExecute() : null)
	{
	}

	public event EventHandler? CanExecuteChanged
	{
		add => CommandManager.RequerySuggested += value;
		remove => CommandManager.RequerySuggested -= value;
	}

	public bool CanExecute(object? parameter)
		=> Interlocked.Read(ref _isExecuting) == 0 && (_canExecute == null || _canExecute(parameter));

	public async void Execute(object? parameter)
	{
		if (!CanExecute(parameter))
		{
			return;
		}

		Interlocked.Exchange(ref _isExecuting, 1);
		RaiseCanExecuteChanged();

		try
		{
			await _execute(parameter);
		}
		finally
		{
			Interlocked.Exchange(ref _isExecuting, 0);
			RaiseCanExecuteChanged();
		}
	}

	public void RaiseCanExecuteChanged()
		=> CommandManager.InvalidateRequerySuggested();
}