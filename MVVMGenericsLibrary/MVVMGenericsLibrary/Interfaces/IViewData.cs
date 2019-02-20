using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMGenericsLibrary.Interfaces
{
    public interface IViewData<TKey>
    {
        TKey Key { get; set; }
    }
}
