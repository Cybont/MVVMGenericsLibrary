using MVVMGenericsLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

// * = Experimental implementations * //

namespace GenericsLibrary
{
    public abstract class MasterDetailsViewModelBase<T, TKey> : IDataPackage<TKey>, INotifyPropertyChanged
    where T : IKey<TKey>, new()
    {
        #region Instance fields
        protected ICRUD<T, TKey> _catalog;
        protected ViewModelFactoryBase<T, TKey> _factoryVM;
        protected ItemViewModelBase<T, TKey> _itemViewModelSelected;
        protected IViewData<TKey> _dataPackage;
        protected DeleteCommandBase<T, TKey> _deleteCommand;
        protected CreateCommandBase<T, TKey> _createCommand;
        protected UpdateCommandBase<T, TKey> _updateCommand;
        protected bool _isItemSelected;
        protected bool _canCreate;
        #endregion

        #region Constructor
        protected MasterDetailsViewModelBase(ViewModelFactoryBase<T, TKey> factoryVM, ICRUD<T, TKey> catalog)
        {
            _catalog = catalog;
            _factoryVM = factoryVM;
        }
        #endregion

        #region Properties for Data Binding

        #region Commands
        public ICommand DeletionCommand => _deleteCommand;
        public ICommand CreateCommand => _createCommand;
        public ICommand UpdateCommand => _updateCommand;
        #endregion

        public abstract IViewData<TKey> DataPackage { get; set; }
       
        public List<ItemViewModelBase<T, TKey>> ItemViewModelCollection => _factoryVM.GetItemViewModelCollection(_catalog);
        public bool IsItemSelected
        {
            get { return _isItemSelected; }
            set
            {
                _isItemSelected = value;
                OnPropertyChanged();
                //This is not abstract
                OnPropertyChanged(nameof(CanCreate));
            }
        }

        public ICRUD<T, TKey> Catalog
        {
            get { return _catalog; }
        }

        public virtual bool CanCreate
        {
            get
            {
                if (!IsItemSelected)
                {
                    _canCreate = true;
                }
                else
                {
                    _canCreate = false;
                }
                return _canCreate;
            }

        }
        public ItemViewModelBase<T, TKey> ItemViewModelSelected
        {
            get
            {
                if (_itemViewModelSelected != null)
                {
                    return _itemViewModelSelected;
                }
                return _itemViewModelSelected = new ItemViewModelBase<T, TKey>(new T());
            } 
            set
            {
                IsItemSelected = true;
                _itemViewModelSelected = value;
                OnPropertyChanged();
                SelectedItemEvent();
            }
        }

        #endregion
        #region Methods
        public void RefreshItemViewModelCollection()
        {
            OnPropertyChanged(nameof(ItemViewModelCollection));
        }
       
        public abstract void SelectedItemEvent();

        #endregion
        #region OnPropertyChanged code
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


    }
}
