using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged
{
    /// <summary>
    /// 方法级别的NPCA切面
    /// </summary>
    [Serializable, AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class NPCA_MethodAttribute : OnMethodBoundaryAspect
    {
        private string _propertyName;

        public NPCA_MethodAttribute(string propertyName = null)
        {
            this._propertyName = propertyName;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            Debug.Assert(args.Method.IsSpecialName, "方法应是特殊的set方法");

            if(_propertyName == null)
            {
                // 从特殊的set方法名得到属性名
                _propertyName = args.Method.Name.Substring(4);
            }

            object _preValue = args.Instance.GetType().GetProperty(this._propertyName).GetValue(args.Instance, null);

            if (object.Equals(_preValue, args.Arguments[0]))
            {
                args.FlowBehavior = FlowBehavior.Return;
            }
        }
        public override void OnSuccess(MethodExecutionArgs args)
        {
            Debug.Assert(args.Method.IsSpecialName, "方法应是特殊的set方法");

            // 属性名应已被赋值
            Debug.Assert(_propertyName != null);

            ObservableObject instance = args.Instance as ObservableObject;

            Debug.Assert(instance != null, "切面类应继承ObservableObject");

            instance.RaisePropertyChanged(this._propertyName);
        }
    }

}
