namespace Okazuki.MVVMRxSample.ViewModels
{
    using Okazuki.MVVMRxSample.Models;
    using Okazuki.MVVMRxSample.Web;

    /// <summary>
    /// ログイン後の画面のViewModel
    /// </summary>
    public class AViewModel : ViewModelBase
    {
        public AViewModel()
        {
        }

        /// <summary>
        /// めんどくさいのでModelをそのまま晒す
        /// </summary>
        public User User { get { return MVVMRxSampleApplication.Current.Authentication.LoginUser; } }
    }
}
