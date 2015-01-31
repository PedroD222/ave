using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DynamicProxyManager
{
    public class DynamicProxyManager<T>
    {
        private static Type type;
        private static Delegate del;

        public static T With<T>(this T src) where T : class
        {
            type = typeof(T);
            return src;
        }

        public static T On<IEnumerable<U>>(this T src, Delegate mi) where T : class
        {
            del = mi;
            return src;
        }
    }
}