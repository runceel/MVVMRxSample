namespace Okazuki.MVVMRxSample.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
    using Microsoft.Practices.Prism.ViewModel;
    using System.Reactive.Linq;

    /// <summary>
    /// ViewModelの基本クラス
    /// </summary>
    public class ViewModelBase : NotificationObject, INotifyDataErrorInfo
    {
        /// <summary>
        /// エラー情報の管理
        /// </summary>
        private ErrorsContainer<string> errors;

        /// <summary>
        /// 処理中かどうかを表すフラグ
        /// </summary>
        private bool isBusy;

        /// <summary>
        /// ナビゲーションリクエスト
        /// </summary>
        public InteractionRequest<Notification> NavigateRequest { get; private set; }

        /// <summary>
        /// アラートメッセージを出すリクエスト
        /// </summary>
        public InteractionRequest<Notification> AlertRequest { get; private set; }

        /// <summary>
        /// 処理中かどうかを取得・設定する。
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }

            set
            {
                this.Set(() => IsBusy, ref this.isBusy, value);
                this.RaisePropertyChanged(() => IsNotBusy);
            }
        }

        /// <summary>
        /// 処理中ではないかを取得する
        /// </summary>
        public bool IsNotBusy
        {
            get
            {
                return !this.IsBusy;
            }
        }

        protected ViewModelBase()
        {
            this.errors = new ErrorsContainer<string>(this.OnErrorChanged);
            this.NavigateRequest = new InteractionRequest<Notification>();
            this.AlertRequest = new InteractionRequest<Notification>();
        }

        /// <summary>
        /// プロパティのsetterの簡略化用メソッド
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        protected void Set<T>(Expression<Func<T>> propertyExpression, ref T field, T value)
        {
            if (Equals(field, value))
            {
                return;
            }

            field = value;
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            this.RaisePropertyChanged(propertyName);
            this.ValidateProperty(propertyName, value);
        }

        /// <summary>
        /// プロパティの値の検証を行う
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        private void ValidateProperty(string propertyName, object value)
        {
            var context = new ValidationContext(this, null, null)
            {
                MemberName = propertyName
            };
            List<ValidationResult> results = new List<ValidationResult>();
            if (Validator.TryValidateProperty(value, context, results))
            {
                this.errors.ClearErrors(propertyName);
                return;
            }

            this.errors.SetErrors(propertyName, results.Select(r => r.ErrorMessage));
        }

        #region INotifyDataErrorInfo メンバー

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected virtual void OnErrorChanged(string propertyName)
        {
            var h = this.ErrorsChanged;
            if (h != null)
            {
                h(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            return this.errors.GetErrors(propertyName);
        }

        public bool HasErrors
        {
            get { return this.errors.HasErrors; }
        }

        #endregion

        /// <summary>
        /// エラーが無い場合にtrueを返す。(DelegateCommandのCanExecuteにセットして使うことを想定）
        /// </summary>
        /// <returns></returns>
        protected bool IsValid() { return !this.HasErrors; }

        /// <summary>
        /// オブジェクトの検証を行う
        /// </summary>
        public void ValidateObject()
        {
            var context = new ValidationContext(this, null, null);
            List<ValidationResult> results = new List<ValidationResult>();
            if (Validator.TryValidateObject(this, context, results))
            {
                return;
            }

            foreach (var g in results.GroupBy(r => r.MemberNames.First()))
            {
                this.errors.SetErrors(g.Key, g.Select(r => r.ErrorMessage));
            }
        }
    }

    public static class ViewModelExtensions
    {
        public static IObservable<T> BusyProcess<T>(this IObservable<T> self, ViewModelBase viewModel)
        {
            return Observable.Defer<T>(() =>
            {
                viewModel.IsBusy = true;
                return self.Finally(() => viewModel.IsBusy = false);
            });
        }
    }
}
