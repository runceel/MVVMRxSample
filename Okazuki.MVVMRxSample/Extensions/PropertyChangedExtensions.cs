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
using System.ComponentModel;
using System.Reactive.Linq;

namespace Okazuki.MVVMRxSample.Extensions
{
    public static class PropertyChangedExtensions
    {
        public static IObservable<PropertyChangedEventArgs> PropertyChangedAsObservable(this INotifyPropertyChanged self)
        {
            return Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => (s, e) => h(e),
                h => self.PropertyChanged += h,
                h => self.PropertyChanged -= h);
        }
    }
}
