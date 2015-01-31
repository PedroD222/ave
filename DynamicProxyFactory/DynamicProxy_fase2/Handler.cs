using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DynamicProxy_fase2
{
   
    interface IInvocationHandler
    {
        object OnCall(CallInfo info);
    }

    public class LoggerInterceptor : IInvocationHandler
    {

        private long start;
        private Stopwatch watch = new Stopwatch();
        public object OnCall(CallInfo info)
        {
            start = watch.ElapsedTicks;
            // call real method using reflection
            object res = info.TargetMethod.Invoke(info.Target, info.Parameters);
            Console.WriteLine("Executed in {0} ticks", watch.ElapsedTicks - start);
            return res;
        }

    }

    //sem public n funciona
    public class MockInterceptor : IInvocationHandler
    {
        public object OnCall(CallInfo info)
        {
            // just a mock interceptor
            // that always returns the same value
            return "some text";
        }
    }
}
