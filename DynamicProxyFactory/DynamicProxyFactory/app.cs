using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace DynamicProxy
{
    public class Program
    {
	    public class Foo {
            public virtual int DoIt(String v) {
                Console.WriteLine( "AClass.DoIt() with {0}",v );
                return v.Length;
            }
        }
        class LoggerInterceptor : IInvocationHandler {
            private long start;
            private Stopwatch watch = new Stopwatch();
            public object OnCall(CallInfo info) {
                start = watch.ElapsedTicks;
            // call real method using reflection
                object res = info.TargetMethod.Invoke(info.Target,info.Parameters);
                Console.WriteLine("Executed in {0} ticks",watch.ElapsedTicks - start);
                return res;
        }
        }
        public static void Main (string [] args)
        {
            IInvocationHandler logInterceptor =new LoggerInterceptor();
            Foo real = new Foo();
            Foo proxy = (Foo) DynamicProxyFactory.MakeProxy<Foo>(real, logInterceptor);
                proxy.DoIt("12");
        
            /*IInvocationHandler mockInterceptor =new MockInterceptor();
        IHelper p =DynamicProxyFactory.MakeProxy<IHelper>(mockInterceptor);
        string s = p.Operation(new Dictionary<int, string>());*/
        }
    
        interface IHelper {
            string Operation(IDictionary<int, string> param);
        }
        class MockInterceptor : IInvocationHandler
        {
            public object OnCall(CallInfo info)
            {
    // just a mock interceptor
    // that always returns the same value
                return "some text";
            }
        }
    
    }
}