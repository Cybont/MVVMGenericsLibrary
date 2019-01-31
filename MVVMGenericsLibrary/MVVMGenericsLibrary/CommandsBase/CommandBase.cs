using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GenericsLibrary
{
    public abstract class CommandBase<TData, T, TKey> : ICommand
        where TData : IKey<TKey>, new()
        where T : IKey<TKey>, new()
    {
        protected MasterDetailsViewModelBase<TData, T, TKey> _viewModel;
        protected ICRUD<T, TData, TKey> _catalog;
        protected CommandBase(ICRUD<T, TData, TKey> catalog, MasterDetailsViewModelBase<TData, T, TKey> viewModel)
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
