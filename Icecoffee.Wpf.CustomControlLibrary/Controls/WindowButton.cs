using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IceCoffee.Wpf.CustomControlLibrary.Controls
{
    public class WindowButton : Control
    {

        /// <summary>
        /// 内部按钮间隔，默认为0
        /// </summary>
        public string InternalMargin { get; set; } = "0";

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        public event EventHandler ClickCloseButton;

        static WindowButton()
        {            
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowButton), new FrameworkPropertyMetadata(typeof(WindowButton)));
        }

        public WindowButton()
        {
            this.DataContext = this;

            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreWindow));
        }
        
        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Maximized;
        }

        private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Normal;
        }

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            if(ClickCloseButton == null)
            {
                Window.GetWindow(this).Close();
            }
            else
            {
                ClickCloseButton.Invoke(sender, e);
            }
        }       
    }
}
