using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProxyManager
{
    public class Foo
    {
        public virtual int DoIt(String v)
        {
            Console.WriteLine("AClass.DoIt() with {0}", v);
            return v.Length;
        }
    }

    public class App<T>
    {
        public static void Main(string[] args)
        {
            Foo f = new Foo();
            f = DynamicProxyManager<string>.With<string>();
            String ola = "bi";
            String o = "w";
            String [] ol = {o, ola};
            
            MethodInfo m ;
        }
    }
}
