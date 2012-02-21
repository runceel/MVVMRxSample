namespace Okazuki.MVVMRxSample.Models
{
    using System;
    using System.Reactive.Linq;
    using Okazuki.MVVMRxSample.Extensions;
    using Okazuki.MVVMRxSample.Web;
    using Microsoft.Practices.Prism.Events;

    /// <summary>
    /// このサンプルプログラムのModelのルート
    /// </summary>
    public class MVVMRxSampleApplication
    {
        /// <summary>
        /// アプリケーションのインスタンスを取得する
        /// </summary>
        public static MVVMRxSampleApplication Current { get; private set; }

        /// <summary>
        /// アプリケーションの初期化を行う
        /// </summary>
        /// <param name="webContext"></param>
        public static void Initialize(WebContext webContext)
        {
            Current = new MVVMRxSampleApplication(webContext);
        }

        /// <summary>
        /// 認証関連の操作を担当するModelを取得する。
        /// </summary>
        public Authentication Authentication { get; private set; }

        public ApplicationContext Context { get; private set; }

        /// <summary>
        /// WebContextを元にアプリケーションを作成します。
        /// </summary>
        /// <param name="webContext"></param>
        public MVVMRxSampleApplication(WebContext webContext)
        {
            this.Context = new ApplicationContext(webContext);
            this.Authentication = new Authentication(this.Context);
        }
    }
}
