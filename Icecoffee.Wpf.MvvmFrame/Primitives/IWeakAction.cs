using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.Wpf.MvvmFrame.Primitives
{
    interface IWeakAction
    {
        /// <summary>
        /// 弱Action的目标
        /// </summary>
        object Target
        {
            get;
        }

        /// <summary>
        /// 执行一个action.
        /// </summary>
        /// <param name="parameter">作为对象传递的参数，要强制转换为适当的类型。</param>
        void ExecuteWithObject(object parameter);

        /// <summary>
        /// 删除所有引用，这将通知清除方法必须删除此项。
        /// </summary>
        void MarkForDeletion();
        
        bool IsAlive
        {
            get;
        }
    }
}
