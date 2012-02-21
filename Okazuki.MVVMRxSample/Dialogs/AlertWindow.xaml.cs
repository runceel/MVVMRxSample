namespace Okazuki.MVVMRxSample.Dialogs
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// アラートダイアログ
    /// </summary>
    public partial class AlertWindow : ChildWindow
    {
        public AlertWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

