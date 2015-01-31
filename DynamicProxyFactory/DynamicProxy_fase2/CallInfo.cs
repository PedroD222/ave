using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DynamicProxy_fase2
{
    public class CallInfo
    {
        private MethodInfo _targetMethod;
        private object _target;
        private object[] _parameters;

        public CallInfo(MethodInfo tm, object t, object[] pm)
        {
            _targetMethod = tm;
            _target = t;
            _parameters = pm;
        }
        public MethodInfo TargetMethod
        {
            get { return _targetMethod; }
        }

        public object Target
        {
            get { return _target; }
        }
        public object[] Parameters
        {
            get { return _parameters; }
        }
    }

}
