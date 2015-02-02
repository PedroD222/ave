using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProxy
{
    public interface IInvocationHandler
    {
        object OnCall(CallInfo info);
    }

}
