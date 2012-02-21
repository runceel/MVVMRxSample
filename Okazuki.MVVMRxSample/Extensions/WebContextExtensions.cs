namespace Okazuki.MVVMRxSample.Extensions
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using Okazuki.MVVMRxSample.Web;

    /// <summary>
    /// WebContextの拡張メソッド
    /// </summary>
    public static class WebContextExtensions
    {
        /// <summary>
        /// WebContextのログインを行う。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="userName">ユーザ名</param>
        /// <param name="password">パスワード</param>
        /// <returns>ユーザ情報。ログインできていない場合はnull。</returns>
        public static IObservable<User> LoginAsObservable(this WebContext self, string userName, string password)
        {
            // Subscribeまで処理を遅延させるためDeferでくるむ
            return Observable.Defer(() =>
            {
                var async = new AsyncSubject<User>();
                // ログインを行う
                var op = self.Authentication.Login(userName, password);
                // Completedイベントを購読して
                Observable.FromEvent<EventHandler, EventArgs>(
                    h => (s, e) => h(e),
                    h => op.Completed += h,
                    h => op.Completed -= h)
                    // 1回のイベント発火でイベントハンドラを解除する
                    .Take(1)
                    .Subscribe(_ =>
                    {
                        // キャンセルされてたら何もせず終了
                        if (op.IsCanceled)
                        {
                            async.OnCompleted();
                            return;
                        }

                        // エラーの場合はエラー通知
                        if (op.HasError)
                        {
                            op.MarkErrorAsHandled();
                            async.OnError(op.Error);
                            return;
                        }

                        // ユーザ情報を発行して終了。
                        async.OnNext(op.User as User);
                        async.OnCompleted();
                    });
                return async.AsObservable();
            });
        }
    }
}
