using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DynamicProxyManager
{
    public interface IProxyMethodInfoBase
    {
        Delegate GetBefore();
        Delegate GetAfter();
        Delegate GetReplace();
        MethodInfo GetMethod();
        void DoBefore(Delegate a);
        void DoAfter(Delegate a);
        void Replace(Delegate f);
    }

    public class ProxyMethodInfo : IProxyMethodInfoBase
    {
        private MethodInfo method;

        private Delegate before;
        private Delegate replace;
        private Delegate after;


        public ProxyMethodInfo(Delegate method)
        {
            this.method = method.Method;
        }

        public void DoBefore(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(method.GetParameters(), ParameterNameComparer.comparer))
                before = Delegate.Combine(before, del);
        }

        public void DoAfter(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(method.GetParameters(), ParameterNameComparer.comparer))
                after = Delegate.Combine(after, del);
        }

        public void Replace(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(method.GetParameters(), ParameterNameComparer.comparer))
                replace = del;
        }

        public Delegate GetBefore()
        {
            return before;
        }

        public Delegate GetAfter()
        {
            return after;
        }

        public Delegate GetReplace()
        {
            return replace;
        }

        public MethodInfo GetMethod()
        {
            return method;
        }
    }

    public class ProxyMethodInfo<T> : IProxyMethodInfoBase
    {
        private MethodInfo method;

        private Delegate before;
        private Delegate replace;
        private Delegate after;


        public ProxyMethodInfo(Func<T> method)
        {
            this.method = method.Method;
        }

        public void DoBefore(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(method.GetParameters(), ParameterNameComparer.comparer))
                before = Delegate.Combine(before, del);
        }

        public void DoAfter(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(method.GetParameters(), ParameterNameComparer.comparer))
                after = Delegate.Combine(after, del);
        }

        public void Replace(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(method.GetParameters(), ParameterNameComparer.comparer))
                replace = del;
        }

        public Delegate GetBefore()
        {
            return before;
        }

        public Delegate GetAfter()
        {
            return after;
        }

        public Delegate GetReplace()
        {
            return replace;
        }

        public MethodInfo GetMethod()
        {
            return method;
        }
    }
    
    public class ProxyMethodInfo<T0, T1> : IProxyMethodInfoBase
    {
        private MethodInfo method;

        private Delegate before;
        private Delegate replace;
        private Delegate after;


        public ProxyMethodInfo(Func<T0, T1> method)
        {
            this.method = method.Method;
        }

        public void DoBefore(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(method.GetParameters(), ParameterNameComparer.comparer))
                before = Delegate.Combine(before, del);
        }

        public void DoAfter(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(method.GetParameters(), ParameterNameComparer.comparer))
                after = Delegate.Combine(after, del);
        }

        public void Replace(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(method.GetParameters(), ParameterNameComparer.comparer))
                replace = del;
        }

        public Delegate GetBefore()
        {
            return before;
        }

        public Delegate GetAfter()
        {
            return after;
        }

        public Delegate GetReplace()
        {
            return replace;
        }

        public MethodInfo GetMethod()
        {
            return method;
        }
    }

    public class ProxyMethodInfo<T0, T1, T2> : IProxyMethodInfoBase
    {
        private MethodInfo method;

        private Delegate before;
        private Delegate replace;
        private Delegate after;


        public ProxyMethodInfo(Func<T0, T1, T2> method)
        {
            this.method = method.Method;
        }

        public void DoBefore(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(method.GetParameters(), ParameterNameComparer.comparer))
                before = Delegate.Combine(before, del);
        }

        public void DoAfter(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(method.GetParameters(), ParameterNameComparer.comparer))
                after = Delegate.Combine(after, del);
        }

        public void Replace(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(method.GetParameters(), ParameterNameComparer.comparer))
                replace = del;
        }

        public Delegate GetBefore()
        {
            return before;
        }

        public Delegate GetAfter()
        {
            return after;
        }

        public Delegate GetReplace()
        {
            return replace;
        }

        public MethodInfo GetMethod()
        {
            return method;
        }
    }

    public class ProxyMethodInfo<T0, T1, T2, T3> : IProxyMethodInfoBase
    {
        private MethodInfo method;

        private Delegate before;
        private Delegate replace;
        private Delegate after;


        public ProxyMethodInfo(Func<T0, T1, T2, T3> method)
        {
            this.method = method.Method;
        }

        public void DoBefore(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(method.GetParameters(), ParameterNameComparer.comparer))
                before = Delegate.Combine(before, del);
        }

        public void DoAfter(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(method.GetParameters(), ParameterNameComparer.comparer))
                after = Delegate.Combine(after, del);
        }

        public void Replace(Delegate del)
        {
            if (del.GetMethodInfo().GetParameters().SequenceEqual(method.GetParameters(), ParameterNameComparer.comparer))
                replace = del;
        }

        public Delegate GetBefore()
        {
            return before;
        }

        public Delegate GetAfter()
        {
            return after;
        }

        public Delegate GetReplace()
        {
            return replace;
        }

        public MethodInfo GetMethod()
        {
            return method;
        }
    }
}
