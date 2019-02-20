using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GenericsLibrary
{
    public class DeleteCommandBase<T, TKey> : CommandBase<T, TKey>
        where T : IKey<TKey>, new()
    {
        public DeleteCommandBase(ICRUD<T, TKey> catalog, MasterDetailsViewModelBase<T, TKey> viewModel)
        :base(catalog, viewModel){}
        public override void ExecuteEvent()
        {
            _viewModel.ItemViewModelSelected = null;
            _viewModel.RefreshItemViewModelCollection();
            _viewModel.IsItemSelected = false;
        }
        public override bool CanExecute()
        {
            return _viewModel.IsItemSelected;
        }
        public override async void Execute()
        {
            await _catalog.Delete(_viewModel.ItemViewModelSelected.Obj.Key);
            ExecuteEvent();
        }
    }
}
