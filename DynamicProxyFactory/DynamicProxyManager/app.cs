using System;
using System.Collections.Generic;
using System.Reflection;

namespace DynamicProxyManager
{
    //sem public n funciona
    public interface IHelper
    {
        string Operation(IDictionary<int, string> param);
    }

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
            Foo proxy = DynamicProxyFactory.MakeProxy<Foo>(real, logInterceptor);
            proxy = DynamicProxyFactory
                .With<Foo>(real)
                .On<String, int>(proxy.DoIt)
                .DoAfter<String>(v => Console.WriteLine("AFTER1 {0}", v))
                .DoBefore<String>(v => Console.WriteLine("BEFORE1 with {0}", v))                
                .DoAfter<String>(v => Console.WriteLine("AFTER2 {0}", v))
                .DoBefore<String>(v => Console.WriteLine("BEFORE2 with {0}", v))                
                .Make();
            int n = proxy.DoIt("cenas");
            Console.WriteLine("Proxy.Doit : "+ n );
            
            IInvocationHandler mockInterceptor =new MockInterceptor();
            IHelper p = (IHelper)DynamicProxyFactory.MakeProxy<IHelper>(mockInterceptor);
            string s = p.Operation(new Dictionary<int, string>());
            Console.WriteLine(s);
            Console.ReadKey();
        }
    }
}