using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace IceCoffee.Wpf.MvvmFrame
{
    /// <summary>
    /// 可观察对象，这是任何提供属性更改通知的对象的抽象基类。
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region 构造方法
        protected ObservableObject()
        {
        }

        #endregion

        #region RaisePropertyChanged
        /// <summary>
        /// 引发此对象的PropertyChanged事件
        /// </summary>
        /// <param name="propertyName">具有新值的属性</param>
        internal protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region 事件
        /// <summary>
        /// 当此对象的属性具有新值时引发
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}