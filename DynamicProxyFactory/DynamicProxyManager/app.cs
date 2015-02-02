using System;
using System.Collections.Generic;
using System.Reflection;

namespace DynamicProxyManager
{

    public class Program
    {
	    public class Foo {
            public virtual int DoIt(String v) {
                Console.WriteLine( "AClass.DoIt() with {0}",v );
                return v.Length;
            }
        }

        public class FooVoid
        {
            public virtual void DoIt()
            {
                Console.WriteLine("AClass.DoIt()");
            }
        }

        public class FooWithDoitTwoP
        {
            public virtual int DoIt(String v, String s)
            {
                Console.WriteLine("AClass.DoIt() with {0} - {1}", v, s);
                return v.Length + s.Length;
            }
        }

        public class FooWithDoitThreeP
        {
            public virtual int DoIt(String v, String v2, String s)
            {
                Console.WriteLine("AClass.DoIt() with {0} - {1} - {2}", v, v2, s);
                return v.Length + v2.Length + s.Length;
            }
        }

        public class FooWithDoitVoidRet
        {
            public virtual String DoIt()
            {
                return "AClass.DoIt()";
            }
        }
        public static void Main (string [] args)
        {
            String s1 = "AVE", s2 = "SI", s3 = "14/15" ;

            Foo real = new Foo();
            real = DynamicProxyFactory
                .With<Foo>(real)
                .On<String, int>(real.DoIt)
                .DoAfter<String>(v => Console.WriteLine("AFTER1 {0}", v))
                .DoBefore<String>(v => Console.WriteLine("BEFORE1 with {0}", v))                
                .DoAfter<String>(v => Console.WriteLine("AFTER2 {0}", v))
                .DoBefore<String>(v => Console.WriteLine("BEFORE2 with {0}", v))                
                .Make();
            int n = real.DoIt("cenas");
            Console.WriteLine("Proxy.Doit : "+ n );
            Console.WriteLine();
            Foo real2 = new Foo();
            real2 = DynamicProxyFactory
                .With<Foo>(real2)
                .On<String, int>(real2.DoIt)
                .DoAfter<String>(v => Console.WriteLine("AFTER1 {0}", v))
                .On<String>(real2.ToString)
                .DoBefore( () => Console.WriteLine("BEFORE real.toSt"))
                .Make();
            real2.DoIt("12");
            real2.ToString();
            Console.WriteLine();
            FooWithDoitVoidRet proxy = new FooWithDoitVoidRet();
            proxy = DynamicProxyFactory.With<FooWithDoitVoidRet>(proxy)
                                        .On<String>(proxy.DoIt)
                                        .DoAfter<String>(v => Console.WriteLine("AFTER1 {0}", v))
                                        .DoBefore<String>(v => Console.WriteLine("BEFORE1 with {0}", v)).Make();
            Console.WriteLine(proxy.DoIt());
            Console.WriteLine();
            FooWithDoitTwoP proxy2 = new FooWithDoitTwoP();
            proxy2 = DynamicProxyFactory.With<FooWithDoitTwoP>(proxy2)
                                        .On<String, String, int>(proxy2.DoIt)
                                        .DoAfter<String>(v => Console.WriteLine("AFTER1 {0}", v))
                                        .Make();
            proxy2.DoIt(s1, s2);
            Console.WriteLine();
            FooWithDoitThreeP proxy3 = new FooWithDoitThreeP();
            proxy3 = DynamicProxyFactory.With<FooWithDoitThreeP>(proxy3)
                                        .On<String, String, String, int>(proxy3.DoIt)
                                        .DoBefore<String>(v => Console.WriteLine("BEFORE1 with {0}", v)).Make();
            proxy3.DoIt(s1, s2, s3);
            Console.WriteLine();
            FooVoid proxy4 = new FooVoid();
            proxy4 = DynamicProxyFactory.With<FooVoid>(proxy4)
                                        .On(proxy4.DoIt)
                                        .Replace(() => Console.Beep(20,200))
                                        .Make();
            proxy4.DoIt();
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}