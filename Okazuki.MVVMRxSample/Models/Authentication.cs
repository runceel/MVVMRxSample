namespace Okazuki.MVVMRxSample.Models
{
    using System;
    using System.Reactive.Linq;
    using Okazuki.MVVMRxSample.Extensions;
    using Okazuki.MVVMRxSample.Web;
    using System.Reactive;
    using Microsoft.Practices.Prism.ViewModel;

    /// <summary>
    /// クライアントサイドの認証を管理するクラス
    /// </summary>
    public class Authentication : NotificationObject
    {
        private ApplicationContext context;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="webContext"></param>
        public Authentication(ApplicationContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// ログインを行う。戻り値に対してSubscribeを行うことでログインが実行される。
        /// </summary>
        /// <param name="userName">ユーザ名</param>
        /// <param name="password">パスワード</param>
        /// <returns>ログインの成功時にはtrueを失敗時にはfalseを発行するIObservable&lt;bool&gt;</returns>
        public IObservable<bool> Login(string userName, string password)
        {
            return this.context
                .Context
                .LoginAsObservable(userName, password)
                .Select(user => user != null && user.IsAuthenticated)
                .Do(_ => this.RaisePropertyChanged("LoginUser", "IsLogin"));
        }

        /// <summary>
        /// ログインユーザ情報を取得する。
        /// </summary>
        public User LoginUser
        {
            get { return this.context.Context.User; }
        }

        /// <summary>
        /// ログイン中かどうかを取得する。
        /// </summary>
        public bool IsLogin
        {
            get { return this.LoginUser != null && this.LoginUser.IsAuthenticated; }
        }
    }
}
