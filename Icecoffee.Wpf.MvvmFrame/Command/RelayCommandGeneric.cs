using System;
using System.Diagnostics;
using System.Windows.Input;

namespace IceCoffee.Wpf.MvvmFrame.Command
{
    /// <summary>
    /// 转发命令，其唯一目的是通过调用委托将其功能传递给其他对象。CanExecute方法的默认返回值为“true”。
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        #region Fields

        private readonly Action<T> _execute = null;
        private readonly Predicate<T> _canExecute = null;

        #endregion

        #region 构造方法

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// 创建一个新的命令
        /// </summary>
        /// <param name="execute">执行逻辑</param>
        /// <param name="canExecute">执行状态逻辑</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        /// <summary>
        /// 当出现影响是否应执行该命令的更改时发生。
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        #endregion        
    }

}
