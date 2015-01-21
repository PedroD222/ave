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

        public static T MakeProxy<T>(T oBase, IInvocationHandler handler)  {
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
                ParameterInfo[] mparams = mInfo.GetParameters();
                Type[] ParametersTypes = new Type[mparams.Length];
                for(int i = 0; i < mparams.Length; ++i){
                    ParametersTypes[i] = mparams[i].ParameterType;
                }                
                MethodBuilder methodBuilder = tb.DefineMethod(mInfo.Name, mInfo.Attributes, mInfo.CallingConvention, mInfo.ReturnType, ParametersTypes);
           
                //build CallInfo

                ILGenerator methodBuilderIL = methodBuilder.GetILGenerator();
                methodBuilderIL.Emit(OpCodes.Mkrefany, mInfo);
                methodBuilderIL.Emit(OpCodes.Ldarg_0);
               
                
                methodBuilderIL.Emit(OpCodes.Ldc_I4, mparams.Length);
                methodBuilderIL.Emit(OpCodes.Newarr, typeof(object));
                for (int i = 0; i < mparams.Length; ++i)
                {
                    methodBuilderIL.Emit(OpCodes.Ldc_I4, i);
                    methodBuilderIL.Emit(OpCodes.Ldarg, i);
                    methodBuilderIL.Emit(OpCodes.Stelem_Ref);
                }
                methodBuilderIL.Emit(OpCodes.Ldloc_0);
                
                Type[] callInfoParamTypes = { typeof(MethodInfo), typeof(object), typeof(object[]) };
                methodBuilderIL.Emit(OpCodes.Call, typeof(CallInfo).GetConstructor(callInfoParamTypes));
                
                //call handler.OnCall(CallInfo)
                //define override
                tb.DefineMethodOverride(methodBuilder, mInfo);
            }

            return (T)Convert.ChangeType(tb.CreateType(), typeof(T));
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