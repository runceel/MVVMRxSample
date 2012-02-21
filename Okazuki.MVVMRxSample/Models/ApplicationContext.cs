namespace Okazuki.MVVMRxSample.Models
{
    using Microsoft.Practices.Prism.Events;

    public class ApplicationContext
    {
        public IEventAggregator EventAggregator { get; private set; }
        public WebContext Context { get; private set; }

        public ApplicationContext(WebContext context)
        {
            this.Context = context;
            this.EventAggregator = new EventAggregator();
        }
    }
}
