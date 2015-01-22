using System;
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
       
        public static void Main (string [] args)
        {
            IInvocationHandler logInterceptor = new LoggerInterceptor();
            Foo real = new Foo();
            Foo proxy = (Foo) DynamicProxyFactory.MakeProxy<Foo>(real, logInterceptor);
            int n = proxy.DoIt("12");
            Console.WriteLine("Proxy.Doit : "+ n );
            
            IInvocationHandler mockInterceptor =new MockInterceptor();
            IHelper p = (IHelper)DynamicProxyFactory.MakeProxy<IHelper>(mockInterceptor);
            string s = p.Operation(new Dictionary<int, string>());
        }
    
        interface IHelper {
            string Operation(IDictionary<int, string> param);
        }
         
    }
}