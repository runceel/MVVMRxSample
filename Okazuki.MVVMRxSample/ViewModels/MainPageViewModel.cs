using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Okazuki.MVVMRxSample.Models;
using Okazuki.MVVMRxSample.Extensions;
using System.Reactive.Linq;

namespace Okazuki.MVVMRxSample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public bool IsLogin { get { return MVVMRxSampleApplication.Current.Authentication.IsLogin; } }

        public MainPageViewModel()
        {
            var model = MVVMRxSampleApplication.Current;
            if (model == null)
            {
                return;
            }

            model.Authentication.PropertyChangedAsObservable()
                .Where(e => e.PropertyName == "IsLogin")
                .Subscribe(e => this.RaisePropertyChanged(e.PropertyName));
        }
    }
}
