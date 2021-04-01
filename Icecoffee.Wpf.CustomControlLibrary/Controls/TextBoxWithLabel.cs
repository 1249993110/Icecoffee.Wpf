using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

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
    ///     <MyNamespace:TextBoxWithLabel/>
    ///
    /// </summary>
    public class TextBoxWithLabel : Control
    {
        public static readonly DependencyProperty LeftTextProperty =
            DependencyProperty.Register("LeftText", typeof(string), typeof(TextBoxWithLabel));

        public static readonly DependencyProperty RightTextProperty =
            DependencyProperty.Register("RightText", typeof(string), typeof(TextBoxWithLabel));

        public static readonly DependencyProperty BoxTextProperty =
            DependencyProperty.Register("BoxText", typeof(string), typeof(TextBoxWithLabel), new FrameworkPropertyMetadata()
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

        public static readonly DependencyProperty TextBoxWidthProperty =
            DependencyProperty.Register("TextBoxWidth", typeof(double), typeof(TextBoxWithLabel));

        public static readonly DependencyProperty LeftTextForegroundProperty =
            DependencyProperty.Register("LeftTextForeground", typeof(Brush), typeof(TextBoxWithLabel));

        public static readonly DependencyProperty RightTextForegroundProperty =
            DependencyProperty.Register("RightTextForeground", typeof(Brush), typeof(TextBoxWithLabel));

        public string LeftText
        {
            get { return (string)GetValue(LeftTextProperty); }
            set { SetValue(LeftTextProperty, value); }
        }

        public string RightText
        {
            get { return (string)GetValue(RightTextProperty); }
            set { SetValue(RightTextProperty, value); }
        }

        /// <summary>
        /// 获取或设置当前TextBox的文本
        /// </summary>
        public string BoxText
        {
            get { return (string)GetValue(BoxTextProperty); }
            set { SetValue(BoxTextProperty, value); }
        }

        public double TextBoxWidth
        {
            get { return (double)GetValue(TextBoxWidthProperty); }
            set { SetValue(TextBoxWidthProperty, value); }
        }

        public Brush LeftTextForeground
        {
            get { return (Brush)GetValue(LeftTextForegroundProperty); }
            set { SetValue(LeftTextForegroundProperty, value); }
        }

        public Brush RightTextForeground
        {
            get { return (Brush)GetValue(RightTextForegroundProperty); }
            set { SetValue(RightTextForegroundProperty, value); }
        }

        static TextBoxWithLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxWithLabel), new FrameworkPropertyMetadata(typeof(TextBoxWithLabel)));
        }
    }
}