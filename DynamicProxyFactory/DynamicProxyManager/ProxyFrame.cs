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
        private List<ProxyMethodInfo> onlist;

        public ProxyHandler(List<ProxyMethodInfo> f){
            this.onlist = f;
        }
    
        public object OnCall(CallInfo info)
        {
            ProxyMethodInfo method = onlist.FirstOrDefault(m => m.Method.Name == info.TargetMethod.Name);
            if (method == null)
                return info.TargetMethod.Invoke(info.Target, info.Parameters);
            if(method.Before != null)
                method.Before.DynamicInvoke(info.Parameters);
            object retVal;
            if (method.ToReplace == null)
                retVal = info.TargetMethod.Invoke(info.Target, info.Parameters);
            else
                retVal = method.ToReplace.DynamicInvoke(info.Parameters);
            if(method.After != null)
                method.After.DynamicInvoke(info.Parameters);

            return retVal;
        }

    }

    public class ProxyFrame<T> where T : class
    {
        private T oBase;
        public List<ProxyMethodInfo> onList;
        private ProxyMethodInfo currentOn;

        public ProxyFrame(T obase)
        {
            this.oBase = obase;
            onList = new List<ProxyMethodInfo>();
        }

        public ProxyFrame<T> On(Action method)
        {
            ProxyMethodInfo pmi = new ProxyMethodInfo(method);
            currentOn = pmi;
            onList.Add(pmi);
            return this;
        }

        public ProxyFrame<T> On<T0>(Func<T0> method)
        {
            ProxyMethodInfo pmi = new ProxyMethodInfo(method);
            currentOn = pmi;
            onList.Add(pmi);
            return this;
        }

        public ProxyFrame<T> On<T0>(Action<T0> method)
        {
            ProxyMethodInfo pmi = new ProxyMethodInfo(method);
            currentOn = pmi;
            onList.Add(pmi);
            return this;
        }

        public ProxyFrame<T> On<T0, T1>(Func<T0, T1> method)
        {
            ProxyMethodInfo pmi = new ProxyMethodInfo(method);
            currentOn = pmi;
            onList.Add(pmi);
            return this;
        }

        public ProxyFrame<T> On<T0, T1>(Action<T0, T1> method)
        {
            ProxyMethodInfo pmi = new ProxyMethodInfo(method);
            currentOn = pmi;
            onList.Add(pmi);
            return this;
        }

        public ProxyFrame<T> On<T0, T1, T2>(Func<T0, T1, T2> method)
        {
            ProxyMethodInfo pmi = new ProxyMethodInfo(method);
            currentOn = pmi;
            onList.Add(pmi);
            return this;
        }

        public ProxyFrame<T> On<T0, T1, T2>(Action<T0, T1, T2> method)
        {
            ProxyMethodInfo pmi = new ProxyMethodInfo(method);
            currentOn = pmi;
            onList.Add(pmi);
            return this;
        }

        public ProxyFrame<T> On<T0, T1, T2, T3>(Func<T0, T1, T2, T3> method)
        {
            ProxyMethodInfo pmi = new ProxyMethodInfo(method);
            currentOn = pmi;
            onList.Add(pmi);
            return this;
        }

        public ProxyFrame<T> DoBefore(Action method)
        {
            currentOn.DoBefore(method);
            return this;
        }

        public ProxyFrame<T> DoBefore<T0>(Action<T0> method)
        {
            currentOn.DoBefore(method);
            return this;
        }

        public ProxyFrame<T> DoBefore<T0, T1>(Action<T0, T1> method)
        {
            currentOn.DoBefore(method);
            return this;
        }

        public ProxyFrame<T> DoBefore<T0, T1, T2>(Action<T0, T1, T2> method)
        {
            currentOn.DoBefore(method);
            return this;
        }

        public ProxyFrame<T> DoAfter(Action method)
        {
            currentOn.DoAfter(method);
            return this;
        }

        public ProxyFrame<T> DoAfter<T0>(Action<T0> method)
        {
            currentOn.DoAfter(method);
            return this;
        }

        public ProxyFrame<T> DoAfter<T0, T1>(Action<T0, T1> method)
        {
            currentOn.DoAfter(method);
            return this;
        }

        public ProxyFrame<T> DoAfter<T0, T1, T2>(Action<T0, T1, T2> method)
        {
            currentOn.DoAfter(method);
            return this;
        }

        public ProxyFrame<T> Replace(Action method)
        {
            currentOn.Replace(method);
            return this;
        }

        public ProxyFrame<T> Replace<T0>(Func<T0> method){
            currentOn.Replace(method);
            return this;
        }

        public ProxyFrame<T> Replace<T0>(Action<T0> method)
        {
            currentOn.Replace(method);
            return this;
        }

        public ProxyFrame<T> Replace<T0, T1>(Action<T0, T1> method)
        {
            currentOn.Replace(method);
            return this;
        }


        public ProxyFrame<T> Replace<T0, T1>(Func<T0, T1> method)
        {
            currentOn.Replace(method);
            return this;
        }
    
        public ProxyFrame<T> Replace<T0, T1, T2>(Action<T0, T1, T2> method)
        {
            currentOn.Replace(method);
            return this;
        }

        public ProxyFrame<T> Replace<T0, T1, T2>(Func<T0, T1, T2> method)
        {
            currentOn.Replace(method);
            return this;
        }

        
        public ProxyFrame<T> Replace<T0, T1, T2, T3>(Func<T0, T1, T2, T3> method)
        {
            currentOn.Replace(method);
            return this;
        }

        public T Make(){
            ProxyHandler<T> handler = new ProxyHandler<T>(onList);
            return DynamicProxyFactory.MakeProxy<T>(oBase, handler) as T;
        }
    }

 
}
