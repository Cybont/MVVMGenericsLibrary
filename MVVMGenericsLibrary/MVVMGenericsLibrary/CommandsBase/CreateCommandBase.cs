using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsLibrary
{
    public class CreateCommandBase<T, TKey> : CommandBase<T, TKey>
        where T : IKey<TKey>, new()
    {
        public CreateCommandBase(ICRUD<T, TKey> catalog, MasterDetailsViewModelBase<T, TKey> viewModel)
        :base(catalog, viewModel){}
        public override void ExecuteEvent()
        {
            _viewModel.RefreshItemViewModelCollection();
        }
        public override bool CanExecute()
        {
            return _viewModel.CanCreate;
        }
        public override async void Execute()
        {
            //_catalog.Create(_viewModel.DataPackage);
            await _catalog.Create(_viewModel.DataPackage);
            ExecuteEvent();
        }
    }
}
