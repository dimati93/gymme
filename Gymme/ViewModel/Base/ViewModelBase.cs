using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Gymme.ViewModel.Base
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private readonly Dictionary<string, Command> _commandHolder = new Dictionary<string, Command>();

        public event PropertyChangedEventHandler PropertyChanged;

        public Command GetOrCreateCommand(string commandName, Action command)
        {
            if (!_commandHolder.ContainsKey(commandName))
            {
                _commandHolder.Add(commandName, new Command(o => command()));
            }

            return _commandHolder[commandName];
        }

        public Command GetOrCreateCommand(string commandName, Action<object> command)
        {
            if (!_commandHolder.ContainsKey(commandName))
            {
                _commandHolder.Add(commandName, new Command(command));
            }

            return _commandHolder[commandName];
        }

        public Command GetOrCreateCommand(string commandName, Action<object> command, Predicate<object> canExecute)
        {
            if (!_commandHolder.ContainsKey(commandName))
            {
                _commandHolder.Add(commandName, new Command(command, canExecute));
            }

            return _commandHolder[commandName];
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}