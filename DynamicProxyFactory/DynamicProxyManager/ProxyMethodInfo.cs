using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DynamicProxyManager
{
    public class ProxyMethodInfo
    {
        public MethodInfo Method { get; set; }
        public Delegate Before { get; set; }
        public Delegate After { get; set; }
        public Delegate ToReplace { get; set; }


        public ProxyMethodInfo(Delegate method)
        {
            Method = method.Method;
        }

        public void DoBefore(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(Method.GetParameters(), ParameterNameComparer.comparer))
                Before = Delegate.Combine(Before, del);
        }

        public void DoAfter(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(Method.GetParameters(), ParameterNameComparer.comparer))
                After = Delegate.Combine(After, del);
        }

        public void Replace(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(Method.GetParameters(), ParameterNameComparer.comparer))
                ToReplace = del;
        }

    }
}
