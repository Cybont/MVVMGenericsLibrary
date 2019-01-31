using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blomstertonden
{
    interface IData<TData>
    {
        TData DataPackage { get; set; }
    }
}
