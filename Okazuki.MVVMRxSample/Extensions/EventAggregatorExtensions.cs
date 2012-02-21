namespace Okazuki.MVVMRxSample.Extensions
{
    using System;
    using System.Reactive.Linq;
    using Microsoft.Practices.Prism.Events;

    public static class EventAggregatorExtensions
    {
        public static void PublishPayload<T>(this IEventAggregator self, T payload)
        {
            self.GetEvent<CompositePresentationEvent<T>>().Publish(payload);
        }

        public static IObservable<T> SubscribePayload<T>(this IEventAggregator self)
        {
            return Observable.Create<T>(observer =>
            {
                var token = self.GetEvent<CompositePresentationEvent<T>>()
                    .Subscribe(payload => observer.OnNext(payload));
                return () => self.GetEvent<CompositePresentationEvent<T>>().Unsubscribe(token);
            });
        }
    }
}
