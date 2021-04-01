using System;
using System.Windows;
using System.Windows.Controls;

namespace IceCoffee.Wpf.CustomControlLibrary.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:IceCoffee.Wpf.CustomControlLibrary.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:IceCoffee.Wpf.CustomControlLibrary.Controls;assembly=IceCoffee.Wpf.CustomControlLibrary.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:ButtonGroup/>
    ///
    /// </summary>
    public class ButtonGroupPanel : StackPanel
    {
        public static readonly DependencyProperty SelectedIndexProperty;

        static ButtonGroupPanel()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonGroupPanel), new FrameworkPropertyMetadata(typeof(ButtonGroupPanel)));
            SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(ButtonGroupPanel));
        }

        /// <summary>
        /// 获取或设置当前选择中项的索引
        /// </summary>
        [System.ComponentModel.Bindable(true)]
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// 设置元素属性，应用样式及数据绑定后调用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            foreach (var item in this.Children)
            {
                if (item is RadioButton radioButton)
                {
                    radioButton.Click += OnButton_Click;
                }
            }
        }

        private void OnButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedIndex = this.Children.IndexOf((UIElement)sender);//(int)((RadioButton)sender).Tag;
        }
    }
}