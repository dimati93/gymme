using System;
using System.Windows.Input;

namespace Gymme.ViewModel.Base
{
    public class Command : ICommand
    {
        #region Constructor

        public Command(Action<object> action)
        {
            ExecuteDelegate = action;
        }

        public Command(Action<object> action, Predicate<object> canExecute)
        {
            ExecuteDelegate = action;
            CanExecuteDelegate = canExecute;
        }

        #endregion

        #region Events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Properties

        public Predicate<object> CanExecuteDelegate { get; set; }

        public Action<object> ExecuteDelegate { get; set; }

        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate != null)
            {
                return CanExecuteDelegate(parameter);
            }

            return true;
        }

        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null)
            {
                ExecuteDelegate(parameter);
            }
        }

        #endregion
    }
}