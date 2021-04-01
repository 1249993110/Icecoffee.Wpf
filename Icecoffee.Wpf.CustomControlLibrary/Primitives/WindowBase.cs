using Icecoffee.Wpf.CustomControlLibrary.Utils;
using IceCoffee.Common.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IceCoffee.Wpf.CustomControlLibrary.Primitives
{
    /// <summary>
    /// 默认实现IClearCaller清理ViewModel
    /// </summary>
    public class WindowBase : Window, IClearCaller
    {
        protected override void OnClosed(EventArgs e)
        {
            InvokeClearMethod();
            base.OnClosed(e);
        }

        public virtual void InvokeClearMethod()
        {
            Util.ClearFrameworkElement(this);
        }        
    }
}
