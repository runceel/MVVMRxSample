namespace Okazuki.MVVMRxSample.Behaviors
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Interactivity;
    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

    /// <summary>
    /// InteractionRequestからの通知を受け取ってFrameにナビゲーションを行うアクション
    /// </summary>
    public class NavigateAction : TriggerAction<Frame>
    {
        protected override void Invoke(object parameter)
        {
            var e = parameter as InteractionRequestedEventArgs;
            if (e == null)
            {
                return;
            }

            var uri = e.Context.Content as string;
            if (uri == null)
            {
                return;
            }

            this.AssociatedObject.Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}
