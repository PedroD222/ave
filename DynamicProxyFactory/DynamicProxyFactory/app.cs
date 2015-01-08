using System;

public class Program
{
	public class Foo {
public virtual int DoIt(String v) {
Console.WriteLine(
"AClass.DoIt() with {0}",
v
);
return v.Length;
}
}
class LoggerInterceptor : IInvocationHandler {
private long start;
private Stopwatch watch = new Stopwatch();
public object OnCall(CallInfo info) {
start = watch.ElapsedTicks;
// call real method using reflection
object res = info.TargetMethod.Invoke(
info.Target,
info.Parameters);
Console.WriteLine("Executed in {0} ticks",
watch.ElapsedTicks - start);
return res;
}
}
IInvocationHandler logInterceptor =
new LoggerInterceptor();
Foo real = new Foo();
Foo proxy =
DynamicProxyFactory.MakeProxy<Foo>(
real,
logInterceptor
);
proxy.DoIt(12);
Listagem 2: Criação de proxy para uma instância da classe Foo
Na Listagem 3 é instanciado um proxy que implementa a interface IHelper e cujo comportamento é
especificado pelo tipo MockInterceptor.
interface IHelper {
string Operation(
IDictionary<int, string> param);
}
class MockInterceptor : IInvocationHandler
{
public object OnCall(CallInfo info)
{
// just a mock interceptor
// that always returns the same value
return “some text”;
}
}
IInvocationHandler mockInterceptor =
new MockInterceptor();
IHelper p =
DynamicProxyFactory.MakeProxy<IHelper>(
mockInterceptor
);
string s = p.Operation(
new Dictionary<int, string>());
}
