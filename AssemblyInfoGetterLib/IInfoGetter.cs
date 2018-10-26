using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyInfoGetterLib
{
    public interface IInfoGetter
    {
        Node GetFileInfo(string fileName);
    }
}
