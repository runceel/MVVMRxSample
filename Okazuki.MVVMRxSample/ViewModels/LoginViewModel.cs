namespace Okazuki.MVVMRxSample.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Practices.Prism.Commands;
    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
    using Okazuki.MVVMRxSample.Models;
    using System.Reactive.Linq;
    using Okazuki.MVVMRxSample.Extensions;

    /// <summary>
    /// ログイン画面のViewModel
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private string userName;

        /// <summary>
        /// ユーザID
        /// </summary>
        [Required(ErrorMessage = "ユーザ名を入力してください")]
        public string UserName
        {
            get
            {
                return this.userName;
            }

            set
            {
                this.Set(() => UserName, ref userName, value);
                this.LoginCommand.RaiseCanExecuteChanged();
            }
        }

        private string password;

        /// <summary>
        /// パスワード
        /// </summary>
        [Required(ErrorMessage = "パスワードを入力してください")]
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                this.Set(() => Password, ref password, value);
                this.LoginCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// ログインコマンド
        /// </summary>
        public DelegateCommand LoginCommand { get; private set; }

        public LoginViewModel()
        {
            this.LoginCommand = new DelegateCommand(
                this.LoginExecute,
                this.IsValid);

        }

        /// <summary>
        /// ログイン処理
        /// </summary>
        private void LoginExecute()
        {
            // ログイン処理前に入力値の検証
            this.ValidateObject();
            if (this.HasErrors)
            {
                this.LoginCommand.RaiseCanExecuteChanged();
                return;
            }

            // Modelにログイン処理を依頼
            var model = MVVMRxSampleApplication.Current;
            model.Authentication
                .Login(this.UserName, this.Password)
                // 処理が完了したのでIsBusyをfalseに
                .BusyProcess(this)
                .Subscribe(success =>
                {
                    if (success)
                    {
                        // 成功時にはAView.xamlへ遷移
                        this.NavigateRequest.Raise(
                            new Notification { Content = "/A" });
                        return;
                    }

                    // 失敗を通知
                    this.AlertRequest.Raise(
                        new Notification
                        {
                            Title = "ログイン失敗",
                            Content = "ユーザ名またはパスワードが違います"
                        });
                },
                ex =>
                {
                    // エラーを通知
                    this.AlertRequest.Raise(
                        new Notification
                        {
                            Title = "エラー",
                            Content = "ログイン中にエラーが発生しました"
                        });
                });
        }
    }
}
