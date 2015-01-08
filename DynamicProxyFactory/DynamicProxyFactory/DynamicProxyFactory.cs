using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProxy
{
    public class CallInfo {
        private MethodInfo _targetMethod;
        private object _target;
        private object[] _parameters;

        public CallInfo(MethodInfo tm, object t, object[] pm){
            _targetMethod = tm;
            _target = t;
            _parameters = pm;
        }
        public MethodInfo TargetMethod {
            get{ return _targetMethod; }
        }

        public object Target{
            get{ return _target; }
        }
        public object[]Parameters{
            get{ return _parameters; }
        }
    }

    public interface IInvocationHandler{
        object OnCall(CallInfo info);
    }

    class DynamicProxyFactory
    {
        static T MakeProxy<T>(Object oBase, IInvocationHandler handler)  {
            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(null, AssemblyBuilderAccess.RunAndCollect); //Pode dar problemas
            ModuleBuilder mb = ab.DefineDynamicModule(null);
            TypeBuilder tb = mb.DefineType(null, TypeAttributes.Public, oBase.GetType());

            foreach (ConstructorInfo cInfo in oBase.GetType().GetConstructors())
            {
                
            }

            foreach (MethodInfo mInfo in oBase.GetType().GetRuntimeMethods())
            {
                //generate method
                //build CallInfo
                //call handler.OnCall(CallInfo)
            }

            return default(T);
        }
    }
}

/*
*class Proxy : Foo
{
    IInvocationHandler handler;
    public Proxy (IInvocationHandler handler){
        this.handler = handler;
    }
    public int DoIt(String s){
        CallInfo ci = new CallInfo(typeof(Foo).GetMethod("DoIt"), this, new object[]{s});
        return (int)handler.OnCall(ci);
    }
}
*/