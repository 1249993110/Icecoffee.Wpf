using IceCoffee.Wpf.CustomControlLibrary.Primitives;
using System;
using System.Windows;

namespace IceCoffee.Wpf.CustomControlLibrary.Controls
{
    /// <summary>
    /// InputDialog.xaml 的交互逻辑
    /// </summary>
    public partial class InputDialog : WindowBase
    {
        public InputDialog(string question, string defaultAnswer = "", string title = "输入")
        {
            InitializeComponent();
            lblQuestion.Content = question;
            txtAnswer.Text = defaultAnswer;
            this.Title = title;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void OnWindow_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }

        public string Answer
        {
            get { return txtAnswer.Text; }
        }
    }
}