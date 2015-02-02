using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;


namespace DynamicProxyManager
{

    class DynamicProxyFactory
    {


        public static T MakeProxy<T>(T oBase, IInvocationHandler handler) where T : class
        {
            Boolean isInterface = oBase == null;
            AssemblyName asn = new AssemblyName("ProxyBuilderAssembly");
            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(asn, AssemblyBuilderAccess.RunAndSave); //Pode dar problemas

            ModuleBuilder mb = ab.DefineDynamicModule(asn.Name, asn.Name + ".dll");

            TypeBuilder tb = mb.DefineType(typeof(T).ToString() + "Proxy", TypeAttributes.Public, !isInterface ? typeof(T) : typeof(object));

            FieldBuilder fReal = null;
            Type[] constructorParameters;
            Object[] constructorArguments;

            if (!isInterface)
            {
                fReal = tb.DefineField("real", oBase.GetType(), FieldAttributes.Private);
                constructorParameters = new Type[] { oBase.GetType(), typeof(IInvocationHandler) };
                constructorArguments = new Object[] { oBase, handler };
            }
            else
            {
                tb.AddInterfaceImplementation(typeof(T));
                constructorParameters = new Type[] { typeof(IInvocationHandler) };
                constructorArguments = new Object[] { handler };
            }


            FieldBuilder fHandler = tb.DefineField("handler", handler.GetType(), FieldAttributes.Private);


            ConstructorBuilder cb = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, constructorParameters);

            ILGenerator cbIL = cb.GetILGenerator();
            cbIL.Emit(OpCodes.Ldarg_0);
            if (!isInterface)
                cbIL.Emit(OpCodes.Call, oBase.GetType().GetConstructor(Type.EmptyTypes));
            else
                cbIL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            cbIL.Emit(OpCodes.Ldarg_0);
            cbIL.Emit(OpCodes.Ldarg_1);
            if (!isInterface)
            {
                cbIL.Emit(OpCodes.Stfld, fReal);
                cbIL.Emit(OpCodes.Ldarg_0);
                cbIL.Emit(OpCodes.Ldarg_2);
            }
            cbIL.Emit(OpCodes.Stfld, fHandler);
            cbIL.Emit(OpCodes.Ret);


            Type[] singleStringType = { typeof(String) };
            MethodInfo getMethod = typeof(Type).GetMethod("GetMethod", singleStringType);
            MethodInfo getType = typeof(Object).GetMethod("GetType");
            foreach (MethodInfo mInfo in typeof(T).GetMethods())
            {
                if (!isInterface && !mInfo.IsVirtual || mInfo.IsConstructor || !mInfo.IsPublic)
                    continue;
                //generate method
                ParameterInfo[] mparams = mInfo.GetParameters();
                Type[] ParametersTypes = new Type[mparams.Length];
                for (int i = 0; i < mparams.Length; ++i)
                {
                    ParametersTypes[i] = mparams[i].ParameterType;
                }
                MethodBuilder methodBuilder;
                if (isInterface)
                    methodBuilder = tb.DefineMethod(mInfo.Name, mInfo.Attributes & ~MethodAttributes.Abstract, mInfo.CallingConvention, mInfo.ReturnType, ParametersTypes);
                else
                    methodBuilder = tb.DefineMethod(mInfo.Name, mInfo.Attributes, mInfo.CallingConvention, mInfo.ReturnType, ParametersTypes);
                //build CallInfo

                ILGenerator methodBuilderIL = methodBuilder.GetILGenerator();

                //create local 2
                LocalBuilder l0 = methodBuilderIL.DeclareLocal(typeof(CallInfo));

                LocalBuilder l2 = methodBuilderIL.DeclareLocal(typeof(object[]));

                methodBuilderIL.Emit(OpCodes.Ldarg_0);
                if (!isInterface)
                    methodBuilderIL.Emit(OpCodes.Ldfld, fReal);
                methodBuilderIL.Emit(OpCodes.Call, getType);
                methodBuilderIL.Emit(OpCodes.Ldstr, mInfo.Name);
                methodBuilderIL.Emit(OpCodes.Call, getMethod);
                methodBuilderIL.Emit(OpCodes.Ldarg_0);

                if (!isInterface)
                {
                    methodBuilderIL.Emit(OpCodes.Ldfld, fReal);
                }
                methodBuilderIL.Emit(OpCodes.Ldc_I4, mparams.Length);
                methodBuilderIL.Emit(OpCodes.Newarr, typeof(object));
                methodBuilderIL.Emit(OpCodes.Stloc_1);
                methodBuilderIL.Emit(OpCodes.Ldloc_1);
                for (int i = 0; i < mparams.Length; ++i)
                {
                    methodBuilderIL.Emit(OpCodes.Dup);
                    methodBuilderIL.Emit(OpCodes.Ldc_I4, i);
                    methodBuilderIL.Emit(OpCodes.Ldarg, i + 1);
                    methodBuilderIL.Emit(OpCodes.Stelem_Ref);
                }
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
                //carregar callinfo
                methodBuilderIL.Emit(OpCodes.Ldloc_0);
                //call handler.OnCall(CallInfo)                
                methodBuilderIL.Emit(OpCodes.Callvirt, handler.GetType().GetMethod("OnCall"));
                if (mInfo.ReturnType.IsValueType && mInfo.ReturnType != typeof(void))
                    methodBuilderIL.Emit(OpCodes.Unbox_Any, mInfo.ReturnType);
                // methodBuilderIL.Emit(OpCodes.Unbox_Any, mInfo.ReturnType);

                methodBuilderIL.Emit(OpCodes.Ret);

                //define override
                tb.DefineMethodOverride(methodBuilder, mInfo);
            }


            Type finishedType = tb.CreateType();
            ab.Save(asn.Name + ".dll");
            ConstructorInfo typeConstructor = finishedType.GetConstructor(constructorParameters);
            object o = typeConstructor.Invoke(constructorArguments);
            return o as T;
        }

        public static T MakeProxy<T>(IInvocationHandler handler) where T : class
        {
            return MakeProxy<T>(null, handler);
        }

        public static ProxyFrame<T> With<T>(T oBase) where T : class
        {
            return new ProxyFrame<T>(oBase);
        }
    }
}
