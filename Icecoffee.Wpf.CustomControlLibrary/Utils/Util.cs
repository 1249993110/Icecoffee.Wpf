using IceCoffee.Common.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Icecoffee.Wpf.CustomControlLibrary.Utils
{
    public static class Util
    {
        /// <summary>
        /// 遍历清理FrameworkElement的ViewModel
        /// </summary>
        /// <param name="frameworkElement"></param>
        public static void ClearFrameworkElement(FrameworkElement frameworkElement)
        {
            if (frameworkElement.DataContext is IClearViewModel clearViewModel)
            {
                clearViewModel.Clear();
            }

            // Image oo = this.Content as Image;
            // MediaElement oo = this.Content as Image;
            // Sharp oo = this.Content as Sharp;

            if (frameworkElement is ItemsControl itemsControl)
            {
                foreach (object uiEl in itemsControl.Items)
                {
                    if (uiEl is FrameworkElement frameEl)
                    {
                        ClearFrameworkElement(frameEl);
                    }
                }
            }
            else if (frameworkElement is ContentControl contentControl)
            {
                object content = contentControl.Content;
                if (content is ItemsControl _itemsControl)
                {
                    foreach (object uiEl in _itemsControl.Items)
                    {
                        if (uiEl is FrameworkElement frameEl)
                        {
                            ClearFrameworkElement(frameEl);
                        }
                    }
                }
                else if (content is Panel panel)
                {
                    foreach (UIElement uiEl in panel.Children)
                    {
                        if (uiEl is FrameworkElement frameEl)
                        {
                            ClearFrameworkElement(frameEl);
                        }
                    }
                }
                else if (content is Decorator decorator)
                {
                    if (decorator.Child is FrameworkElement frameEl)
                    {
                        ClearFrameworkElement(frameEl);
                    }
                }
                else if (content is Control control)// 最基础的控件不需要再递归了
                {
                    if (control.DataContext is IClearViewModel _clearViewModel)
                    {
                        _clearViewModel.Clear();
                    }
                }
            }
            else if (frameworkElement is Control control)// 最基础的控件不需要再递归了
            {
                if (control.DataContext is IClearViewModel _clearViewModel)
                {
                    _clearViewModel.Clear();
                }
            }
        }
    }
}
