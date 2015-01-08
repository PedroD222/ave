using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProxy
{

    public interface IInvocationHandler{
        object OnCall(CallInfo info);
    }

    class DynamicProxyFactory
    {
        public static T MakeProxy<T>(Object oBase, IInvocationHandler handler)  {
            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(null, AssemblyBuilderAccess.RunAndCollect); //Pode dar problemas
            
            ModuleBuilder mb = ab.DefineDynamicModule(null);
            
            TypeBuilder tb = mb.DefineType(null, TypeAttributes.Public, oBase.GetType());

            FieldBuilder fReal = tb.DefineField("real", oBase.GetType(), FieldAttributes.Private);
            FieldBuilder fHandler = tb.DefineField("handler", typeof(IInvocationHandler), FieldAttributes.Private);

            Type[] constructorParameters = {oBase.GetType(), typeof(IInvocationHandler)};
            ConstructorBuilder cb = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, constructorParameters);

            ILGenerator cbIL = cb.GetILGenerator();
            cbIL.Emit(OpCodes.Ldarg_0);
            cbIL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            cbIL.Emit(OpCodes.Ldarg_0);
            cbIL.Emit(OpCodes.Ldarg_1);
            cbIL.Emit(OpCodes.Stfld, fReal);
            cbIL.Emit(OpCodes.Ldarg_2);
            cbIL.Emit(OpCodes.Stfld, fHandler);
            cbIL.Emit(OpCodes.Ret);

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