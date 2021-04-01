using System;
using System.Reflection;

namespace IceCoffee.Wpf.MvvmFrame.Primitives
{
    public class WeakAction<T> : IWeakAction
    {
        #region 字段

        private Action<T> _staticAction;

        private WeakReference _targetRef;

        #endregion 字段

        protected MethodInfo Method
        {
            get;
            set;
        }

        public string MethodName
        {
            get { return Method.Name; }
        }

        public virtual bool IsAlive
        {
            get
            {
                if (_staticAction == null
                    && _targetRef == null)
                {
                    return false;
                }

                if (_staticAction != null)
                {
                    if (_targetRef != null)
                    {
                        return _targetRef.IsAlive;
                    }

                    return true;
                }

                // Non static action

                if (_targetRef != null)
                {
                    return _targetRef.IsAlive;
                }

                return false;
            }
        }

        public WeakAction(Action<T> action)
            : this(action == null ? null : action.Target, action)
        {
        }

        /// <summary>
        /// 构造一个弱操作
        /// </summary>
        /// <param name="target">调用目标方法的对象，如果方法是静态的，则为空</param>
        public WeakAction(object target, Action<T> action)
        {
            if (target == null)
            {
                _targetRef = null;
            }
            else
            {
                _targetRef = new WeakReference(target);
            }

            if (action.Method.IsStatic)
            {
                _staticAction = action;
            }

            Method = action.Method;
        }

        public object Target
        {
            get { return _targetRef.Target; }
        }

        public void ExecuteWithObject(object parameter)
        {
            if (_staticAction != null)
            {
                _staticAction((T)parameter);
                return;
            }

            if (_targetRef.IsAlive)
            {
                Method.Invoke(_targetRef.Target, new object[] { parameter });
            }
        }

        public void MarkForDeletion()
        {
            _staticAction = null;
            _targetRef = null;
        }
    }
}