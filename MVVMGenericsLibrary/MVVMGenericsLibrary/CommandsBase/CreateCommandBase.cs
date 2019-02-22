using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsLibrary
{
    public abstract class CreateCommandBase<T, TKey> : CommandBase<T, TKey>
        where T : IKey<TKey>, new()
    {
        public CreateCommandBase(ICRUD<T, TKey> catalog, MasterDetailsViewModelBase<T, TKey> viewModel)
        :base(catalog, viewModel){}

        public abstract bool GenerateKey { get; set; }

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
            if (GenerateKey) {
                await _catalog.Create(_viewModel.DataPackage, true);
            }
            else
            {
                await _catalog.Create(_viewModel.DataPackage, false);
            }
            
            ExecuteEvent();
        }
    }
}
