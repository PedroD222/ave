﻿using System;
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

        public static object MakeProxy<T>(T oBase, IInvocationHandler handler) where T : class {
            AssemblyName asn = new AssemblyName("ProxyBuilderAssembly");
            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(asn, AssemblyBuilderAccess.RunAndSave); //Pode dar problemas
            
            ModuleBuilder mb = ab.DefineDynamicModule(asn.Name, asn.Name + ".dll");
            
            TypeBuilder tb = mb.DefineType(oBase.GetType().ToString()+"Proxy", TypeAttributes.Public, oBase.GetType());

            FieldBuilder fReal = tb.DefineField("real", oBase.GetType(), FieldAttributes.Private);
            FieldBuilder fHandler = tb.DefineField("handler", typeof(IInvocationHandler), FieldAttributes.Private);

            Type[] constructorParameters = {oBase.GetType(), typeof(IInvocationHandler)};
            ConstructorBuilder cb = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, constructorParameters);

            ILGenerator cbIL = cb.GetILGenerator();
            cbIL.Emit(OpCodes.Ldarg_0);
            cbIL.Emit(OpCodes.Call, oBase.GetType().GetConstructor(Type.EmptyTypes));
            cbIL.Emit(OpCodes.Ldarg_0);
            cbIL.Emit(OpCodes.Ldarg_1);
            cbIL.Emit(OpCodes.Stfld, fReal);
            cbIL.Emit(OpCodes.Ldarg_0);
            cbIL.Emit(OpCodes.Ldarg_2);
            cbIL.Emit(OpCodes.Stfld, fHandler);
            cbIL.Emit(OpCodes.Ret);


            Type[] singleStringType = { typeof(String) };
            MethodInfo getMethod = typeof(Type).GetMethod("GetMethod", singleStringType);
            MethodInfo getType = typeof(Object).GetMethod("GetType");
            foreach (MethodInfo mInfo in oBase.GetType().GetMethods())
            {
                if (!mInfo.IsVirtual || mInfo.IsConstructor)
                    continue;
                //generate method
                ParameterInfo[] mparams = mInfo.GetParameters();
                Type[] ParametersTypes = new Type[mparams.Length];
                for(int i = 0; i < mparams.Length; ++i){
                    ParametersTypes[i] = mparams[i].ParameterType;
                }                
                MethodBuilder methodBuilder = tb.DefineMethod(mInfo.Name, mInfo.Attributes, mInfo.CallingConvention, mInfo.ReturnType, ParametersTypes);
                
                //build CallInfo
               
                ILGenerator methodBuilderIL = methodBuilder.GetILGenerator();
               
                //create local 2
                LocalBuilder l0 = methodBuilderIL.DeclareLocal(typeof(CallInfo));
                LocalBuilder l1 = methodBuilderIL.DeclareLocal(typeof(int));
                LocalBuilder l2 = methodBuilderIL.DeclareLocal(typeof(object []));
                
                methodBuilderIL.Emit(OpCodes.Ldarg_0);
                methodBuilderIL.Emit(OpCodes.Ldfld, fReal);
                methodBuilderIL.Emit(OpCodes.Call, getType);
                methodBuilderIL.Emit(OpCodes.Ldstr, mInfo.Name);
                methodBuilderIL.Emit(OpCodes.Call, getMethod);
         //       methodBuilderIL.Emit(OpCodes.Mkrefany, mInfo.GetType());
                
                methodBuilderIL.Emit(OpCodes.Ldarg_0);
                methodBuilderIL.Emit(OpCodes.Ldfld, fReal);
                
                methodBuilderIL.Emit(OpCodes.Ldc_I4, mparams.Length);
                methodBuilderIL.Emit(OpCodes.Newarr, typeof(object));
                methodBuilderIL.Emit(OpCodes.Stloc_2);
                methodBuilderIL.Emit(OpCodes.Ldloc_2);
                //methodBuilderIL.Emit(OpCodes.Dup);
                for (int i = 0; i < mparams.Length; ++i)
                {
                  //  methodBuilderIL.Emit(OpCodes.Dup);
                    methodBuilderIL.Emit(OpCodes.Ldc_I4, i);
                    methodBuilderIL.Emit(OpCodes.Ldarg, i+1);
                    methodBuilderIL.Emit(OpCodes.Stelem_Ref);
                }
                methodBuilderIL.Emit(OpCodes.Ldloc_2);
                //debug
               // methodBuilderIL.Emit(OpCodes.Ldnull);
                //methodBuilderIL.Emit(OpCodes.Ldnull);
               // methodBuilderIL.Emit(OpCodes.Ldnull);
                Type[] callInfoParamTypes = { typeof(MethodInfo), typeof(object), typeof(object[]) };
                methodBuilderIL.Emit(OpCodes.Newobj, typeof(CallInfo).GetConstructor(callInfoParamTypes));
                methodBuilderIL.Emit(OpCodes.Stloc_0);

                //handler Proxy
                methodBuilderIL.Emit(OpCodes.Ldarg_0);
                methodBuilderIL.Emit(OpCodes.Ldfld, fHandler);
                //carregare callinfo
                methodBuilderIL.Emit(OpCodes.Ldloc_0);
                //call handler.OnCall(CallInfo)                
                //MethodInfo m= handler.GetType().GetMethod("OnCall");
                methodBuilderIL.Emit(OpCodes.Callvirt, handler.GetType().GetMethod("OnCall"));
                
                //methodBuilderIL.Emit(OpCodes.Unbox_Any, typeof(int));*/
                
                methodBuilderIL.Emit(OpCodes.Pop);
                methodBuilderIL.Emit(OpCodes.Ldc_I4_8);
                methodBuilderIL.Emit(OpCodes.Ret);
                
                //define override
                tb.DefineMethodOverride(methodBuilder, mInfo);
                
            }

            
            Type finishedType = tb.CreateType();
            ab.Save(asn.Name + ".dll");
            ConstructorInfo typeConstructor = finishedType.GetConstructor(constructorParameters);
            Object[] constructorArguments = {oBase, handler};
            object o = typeConstructor.Invoke(constructorArguments);
            return o as T;
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