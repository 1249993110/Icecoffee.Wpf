using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;

namespace IceCoffee.Wpf.MvvmFrame
{
    /// <summary>
    /// 监视实现INotifyPropertyChanged的对象的PropertyChanged事件，并执行为该对象的属性注册的回调方法（即处理程序）
    /// </summary>
    /// <typeparam name="TPropertySource">要监视属性更改的对象的类型</typeparam>
    public class PropertyObserver<TPropertySource> : IWeakEventListener
        where TPropertySource : class, INotifyPropertyChanged
    {
        #region Fields
        private readonly Dictionary<string, Action<TPropertySource>> _propertyNameToHandlerMap;
        private readonly WeakReference _propertySourceRef;

        #endregion

        #region 构造方法

        /// <summary>
        /// 初始化PropertyObserver的新实例，该实例观察属性更改的"propertySource"对象。
        /// </summary>
        /// <param name="propertySource">要监视属性更改的对象</param>
        public PropertyObserver(TPropertySource propertySource)
        {
            if (propertySource == null)
                throw new ArgumentNullException("propertySource");

            _propertySourceRef = new WeakReference(propertySource);
            _propertyNameToHandlerMap = new Dictionary<string, Action<TPropertySource>>();
        }

        #endregion

        #region Public Methods

        #region RegisterHandler

        /// <summary>
        /// 注册在为指定属性引发PropertyChanged事件时要调用的回调。
        /// </summary>
        /// <param name="expression">lambda表达式，如 'n => n.PropertyName'</param>
        /// <param name="handler">属性更改时要调用的回调</param>
        /// <returns>调用此方法的对象，以允许将多个调用链接在一起</returns>
        public PropertyObserver<TPropertySource> RegisterHandler(
            Expression<Func<TPropertySource, object>> expression,
            Action<TPropertySource> handler)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            string propertyName = GetPropertyName(expression);
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("'expression' did not provide a property name.");            

            return RegisterHandler(propertyName, handler);
        }

        /// <summary>
        /// 注册在为指定属性引发PropertyChanged事件时要调用的回调。
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="handler">属性更改时要调用的回调</param>
        /// <returns>调用此方法的对象，以允许将多个调用链接在一起</returns>
        public PropertyObserver<TPropertySource> RegisterHandler(string propertyName, Action<TPropertySource> handler)
        {
            //if (string.IsNullOrEmpty(propertyName))
            //    throw new ArgumentException("'propertyName' is null or empty.");

            CheckPropertyName(typeof(TPropertySource), propertyName);

            //if(typeof(TPropertySource).GetProperties().Any(p=>p.Name == propertyName))
            //    throw new ArgumentException("'propertyName' not included in source");

            if (handler == null)
                throw new ArgumentNullException("handler");

            TPropertySource propertySource = this.GetPropertySource();
            if (propertySource != null)
            {
                Debug.Assert(!_propertyNameToHandlerMap.ContainsKey(propertyName), "Why is the '" + propertyName + "' property being registered again?");
                _propertyNameToHandlerMap[propertyName] = handler;
                PropertyChangedEventManager.AddListener(propertySource, this, propertyName);
            }

            return this;
        }

        /// <summary>
        /// 检查属性名
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        [Conditional("DEBUG")]
        private static void CheckPropertyName(Type type, string propertyName)
        {
            typeof(TPropertySource).GetProperty(propertyName);
        }
        #endregion // RegisterHandler

        #region UnregisterHandler

        /// <summary>
        /// 移除与指定属性关联的回调。
        /// </summary>
        /// <param name="propertyName">lambda表达式，如 'n => n.PropertyName'</param>
        /// <returns>调用此方法的对象，以允许将多个调用链接在一起</returns>
        public PropertyObserver<TPropertySource> UnregisterHandler(Expression<Func<TPropertySource, object>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            string propertyName = GetPropertyName(expression);
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("'expression' did not provide a property name.");

             TPropertySource propertySource = this.GetPropertySource();
             if (propertySource != null)
            {
                if (_propertyNameToHandlerMap.ContainsKey(propertyName))
                {
                    _propertyNameToHandlerMap.Remove(propertyName);
                    PropertyChangedEventManager.RemoveListener(propertySource, this, propertyName);
                }
            }

            return this;
        }

        #endregion

        #endregion

        #region IWeakEventListener Members

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType != typeof(PropertyChangedEventManager))
            {
                return false;
            }
            PropertyChangedEventArgs args = e as PropertyChangedEventArgs;
            if (args != null && sender is TPropertySource)
            {
                string propertyName = args.PropertyName;
                TPropertySource propertySource = (TPropertySource)sender;

                if (string.IsNullOrEmpty(propertyName))
                {
                    // 当属性名为空时，所有属性都将被视为无效。迭代处理程序列表的副本，以防回调注册处理程序。
                    foreach (Action<TPropertySource> handler in _propertyNameToHandlerMap.Values.ToArray())
                    {
                        handler(propertySource);
                    }

                    return true;
                }
                else
                {
                    Action<TPropertySource> handler;
                    if (_propertyNameToHandlerMap.TryGetValue(propertyName, out handler))
                    {
                        handler(propertySource);

                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Private Helpers

        #region GetPropertyName

        private static string GetPropertyName(Expression<Func<TPropertySource, object>> expression)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            Debug.Assert(memberExpression != null, "Please provide a lambda expression like 'n => n.PropertyName'");

            if (memberExpression != null)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;
                if (propertyInfo != null)
                {
                    return propertyInfo.Name;
                }
            }

            return null;
        }

        #endregion

        #region GetPropertySource

        private TPropertySource GetPropertySource()
        {
            return _propertySourceRef.IsAlive ? (TPropertySource)_propertySourceRef.Target : null;
        }

        #endregion

        #endregion

    }
}