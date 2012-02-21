namespace Okazuki.MVVMRxSample.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    /// <summary>
    /// PasswordBoxのPasswordプロパティの変更時にバインディングのソースを更新します。
    /// </summary>
    public class UpdatePasswordBindingOnPropertyChanged : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
            base.OnDetaching();
        }

        private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBindingExpression = this.AssociatedObject.GetBindingExpression(PasswordBox.PasswordProperty);
            passwordBindingExpression.UpdateSource();
        }

    }
}
