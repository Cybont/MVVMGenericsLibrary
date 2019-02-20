using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GenericsLibrary
{
    public abstract class CommandBase<T, TKey> : ICommand
        where T : IKey<TKey>, new()
    {
        protected MasterDetailsViewModelBase<T, TKey> _viewModel;
        protected ICRUD<T, TKey> _catalog;
        protected CommandBase(ICRUD<T, TKey> catalog, MasterDetailsViewModelBase<T, TKey> viewModel)
        {
            _catalog = catalog;
            _viewModel = viewModel;
        }
        public abstract void Execute();
        public abstract void ExecuteEvent();
        public virtual bool CanExecute()
        {
            return true;
        }
        public bool CanExecute(object parameter)
        {
            return CanExecute();
        }
        public void Execute(object parameter)
        {
            Execute();
        }
        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
