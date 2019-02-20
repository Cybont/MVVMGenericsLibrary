using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMGenericsLibrary.Interfaces
{
    public interface IDataPackage<TKey>
    {
        IViewData<TKey> DataPackage { get; set; }
    }
}
