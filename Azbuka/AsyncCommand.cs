using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Azbuka
{
     
        public class AsyncCommand : ICommand
        {
            protected readonly Predicate<object> _canExecute;
            protected Func<object, Task> _execute;

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public AsyncCommand(Func<object, Task> execute)
                : this(execute, null)
            {
            }

            public AsyncCommand(Func<object, Task> execute,
                           Predicate<object> canExecute)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                if (_canExecute == null)
                {
                    return true;
                }

                return _canExecute(parameter);
            }

            public async void Execute(object parameter)
            {
                await _execute(parameter);
            }

        }
 

}
