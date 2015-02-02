using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProxyManager
{
    public interface IInvocationHandler
    {
        object OnCall(CallInfo info);
    }

}
