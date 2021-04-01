using IceCoffee.Common.Wpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IceCoffee.Wpf.MvvmFrame
{
    /// <summary>
    /// 可观察对象，这是任何提供属性更改通知的对象的抽象基类。
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged, IClearViewModel
    {
        #region 构造方法

        protected ObservableObject()
        {
        }

        #endregion 构造方法

        #region RaisePropertyChanged

        /// <summary>
        /// 引发此对象的PropertyChanged事件
        /// </summary>
        /// <param name="propertyName">具有新值的属性</param>
        protected internal void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion RaisePropertyChanged

        public virtual void Clear()
        {

        }

        #region 事件

        /// <summary>
        /// 当此对象的属性具有新值时引发
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion 事件
    }
}