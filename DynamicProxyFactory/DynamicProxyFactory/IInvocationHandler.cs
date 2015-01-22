using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DynamicProxy
{
   
    public interface IInvocationHandler
    {
        object OnCall(CallInfo info);
    }

    class LoggerInterceptor
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
}
