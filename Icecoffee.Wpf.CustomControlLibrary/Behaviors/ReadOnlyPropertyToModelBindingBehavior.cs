using System.Windows;

namespace IceCoffee.Wpf.CustomControlLibrary.Behaviors
{
    public class ReadOnlyPropertyToModelBindingBehavior
    {
        public static readonly DependencyProperty ReadOnlyDependencyPropertyProperty = DependencyProperty.RegisterAttached(
           "ReadOnlyDependencyProperty",
           typeof(object),
           typeof(ReadOnlyPropertyToModelBindingBehavior),
           new PropertyMetadata(OnReadOnlyDependencyPropertyPropertyChanged));

        public static void SetReadOnlyDependencyProperty(DependencyObject element, object value)
        {
            element.SetValue(ReadOnlyDependencyPropertyProperty, value);
        }

        public static object GetReadOnlyDependencyProperty(DependencyObject element)
        {
            return element.GetValue(ReadOnlyDependencyPropertyProperty);
        }

        private static void OnReadOnlyDependencyPropertyPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            SetModelProperty(obj, e.NewValue);
        }

        public static readonly DependencyProperty ModelPropertyProperty = DependencyProperty.RegisterAttached(
           "ModelProperty",
           typeof(object),
           typeof(ReadOnlyPropertyToModelBindingBehavior),
           new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static void SetModelProperty(DependencyObject element, object value)
        {
            element.SetValue(ModelPropertyProperty, value);
        }

        public static object GetModelProperty(DependencyObject element)
        {
            return element.GetValue(ModelPropertyProperty);
        }
    }
}