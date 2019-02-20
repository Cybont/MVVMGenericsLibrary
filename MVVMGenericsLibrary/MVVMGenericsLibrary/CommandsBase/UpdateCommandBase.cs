using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsLibrary
{
    public class UpdateCommandBase<T, TKey> : CommandBase<T, TKey>
        where T : IKey<TKey>, new()
    {
        public UpdateCommandBase(ICRUD<T, TKey> catalog, MasterDetailsViewModelBase<T, TKey> viewModel)
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
            //_catalog.Update(_viewModel.DataPackage);
            await _catalog.Update(_viewModel.DataPackage);
            ExecuteEvent();
        }
    }
}
