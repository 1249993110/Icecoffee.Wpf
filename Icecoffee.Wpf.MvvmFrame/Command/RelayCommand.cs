using System;
using System.Diagnostics;
using System.Windows.Input;

namespace IceCoffee.Wpf.MvvmFrame.Command
{
    /// <summary>
    /// 转发命令，其唯一目的是通过调用委托将其功能传递给其他对象。CanExecute方法的默认返回值为“true”。
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        #endregion

        #region Constructors

        /// <summary>
        /// 创建始终可以执行的新命令
        /// </summary>
        /// <param name="execute">执行逻辑</param>
        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// 创建一个新的命令
        /// </summary>
        /// <param name="execute">执行逻辑</param>
        /// <param name="canExecute">执行状态逻辑</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
                
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter = null)
        {
            return _canExecute == null ? true : _canExecute();
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

        /// <summary>
        /// 执行命令
        /// </summary>
        public void Execute(object parameter = null)
        {
            _execute();
        }

        #endregion
    }
}