using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DynamicProxyManager
{

    public class ParameterNameComparer : EqualityComparer<ParameterInfo>
    {

        public override bool Equals(ParameterInfo x, ParameterInfo y)
        {
            return x.ParameterType.Equals(y.ParameterType);
        }

        public override int GetHashCode(ParameterInfo obj)
        {
            return obj.GetHashCode();
        }

        public static ParameterNameComparer comparer = new ParameterNameComparer();
    }

    public class ProxyHandler<T> : IInvocationHandler
    {
        private List<IProxyMethodInfoBase> onlist;

        public ProxyHandler(List<IProxyMethodInfoBase> f){
            this.onlist = f;
        }
    
        public object OnCall(CallInfo info)
        {
            IProxyMethodInfoBase method = onlist.FirstOrDefault(m => m.GetMethod().Name == info.TargetMethod.Name);
            if (method == null)
                return info.TargetMethod.Invoke(info.Target, info.Parameters);
            if(method.GetBefore() != null)
                method.GetBefore().DynamicInvoke(info.Parameters);
            object retVal;
            if (method.GetReplace() == null)
                retVal = info.TargetMethod.Invoke(info.Target, info.Parameters);
            else
                retVal = method.GetReplace().DynamicInvoke(info.Parameters);
            if(method.GetAfter() != null)
                method.GetAfter().DynamicInvoke(info.Parameters);
            return retVal;
        }

    }

    public class ProxyFrame<T> where T : class
    {
        private T oBase;
        public List<IProxyMethodInfoBase> onList;

        public ProxyFrame(T obase)
        {
            this.oBase = obase;
            onList = new List<IProxyMethodInfoBase>();
        }

        public ProxyFrame<T> On(Action method)
        {
            ProxyMethodInfo pmi = new ProxyMethodInfo(method);
            onList.Add(pmi);
            return this;
        }

        public ProxyFrame<T> On<T0>(Func<T0> method)
        {
            ProxyMethodInfo<T0> pmi = new ProxyMethodInfo<T0>(method);
            onList.Add(pmi);
            return this;
        }

        public ProxyFrame<T> On<T0, T1>(Func<T0, T1> method)
        {
            ProxyMethodInfo<T0, T1> pmi = new ProxyMethodInfo<T0,T1>(method);
            onList.Add(pmi);
            return this;
        }

        public ProxyFrame<T> On<T0, T1, T2>(Func<T0, T1, T2> method)
        {
            ProxyMethodInfo<T0, T1, T2> pmi = new ProxyMethodInfo<T0, T1, T2>(method);
            onList.Add(pmi);
            return this;
        }

        public ProxyFrame<T> On<T0, T1, T2, T3>(Func<T0, T1, T2, T3> method)
        {
            ProxyMethodInfo<T0, T1, T2, T3> pmi = new ProxyMethodInfo<T0, T1, T2, T3>(method);
            onList.Add(pmi);
            return this;
        }

        public ProxyFrame<T> DoBefore(Action method)
        {
            onList.Last().DoBefore(method);
            return this;
        }

        public ProxyFrame<T> DoBefore<T0>(Action<T0> method)
        {
            onList.Last().DoBefore(method);
            return this;
        }

        public ProxyFrame<T> DoBefore<T0, T1>(Action<T0, T1> method)
        {
            onList.Last().DoBefore(method);
            return this;
        }

        public ProxyFrame<T> DoBefore<T0, T1, T2>(Action<T0, T1, T2> method)
        {
            onList.Last().DoBefore(method);
            return this;
        }


        public ProxyFrame<T> DoAfter<T0>(Action<T0> method)
        {
            onList.Last().DoAfter(method);
            return this;
        }

        public ProxyFrame<T> Replace<T0, T1>(Func<T0, T1> method)
        {
            onList.Last().Replace(method);
            return this;
        }

        public T Make(){
            ProxyHandler<T> handler = new ProxyHandler<T>(onList);
            return DynamicProxyFactory.MakeProxy<T>(oBase, handler) as T;
        }
    }

 
}
