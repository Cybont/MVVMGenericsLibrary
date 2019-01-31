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
    public abstract class MasterDetailsViewModelBase<TData, T, TKey> : INotifyPropertyChanged
    where TData : IKey<TKey>, new()
    where T : IKey<TKey>, new()
    {
        #region Instance fields
        protected ICRUD<T, TData, TKey> _catalog;
        protected ViewModelFactoryBase<TData, T, TKey> _factoryVM;
        protected ItemViewModelBase<T, TKey> _itemViewModelSelected;
        //protected TData _dataPackage;
        protected DeleteCommandBase<TData, T, TKey> _deleteCommand;
        protected CreateCommandBase<TData, T, TKey> _createCommand;
        protected UpdateCommandBase<TData, T, TKey> _updateCommand;
        protected bool _isItemSelected;
        protected bool _canCreate;
        #endregion
        #region Constructor
        protected MasterDetailsViewModelBase(ViewModelFactoryBase<TData, T, TKey> factoryVM, ICRUD<T, TData, TKey> catalog)
        {
            _catalog = catalog;
            _factoryVM = factoryVM;
            //_dataPackage = new TData();
        }
        #endregion
        #region Properties for Data Binding
        #region Commands
        public ICommand DeletionCommand => _deleteCommand;
        public ICommand CreateCommand => _createCommand;
        public ICommand UpdateCommand => _updateCommand;
        #endregion
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

        public ICRUD<T, TData, TKey> Catalog
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
        //public TData DataPackage {
        //    get => _dataPackage;
        //    set => _dataPackage = value;
        //}
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
