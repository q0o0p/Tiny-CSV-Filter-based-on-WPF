using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Filters
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        private readonly Action execute;
        private readonly Func<Boolean> canExecute;

        public RelayCommand(Action execute) : this(execute, null) { }
        public RelayCommand(Action execute, Func<Boolean> canExecute)
        {
            if (null == execute)
            {
                throw new ArgumentNullException("execute is null!");
            }
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public Boolean CanExecute()
        {
            return null == this.canExecute ? true : this.canExecute(); 
        }

        public void Execute()
        {
            this.execute();
        }

        Boolean ICommand.CanExecute(Object needlessParameter)
        {
            return this.CanExecute();
        }

        void ICommand.Execute(Object needlessParameter)
        {
            this.Execute();
        }
    }
}
