using System;
using System.Windows.Threading;

namespace SniffCore.Collections
{
    public sealed class DispatcherBeginInvokator : IInvokator
    {
        private readonly Dispatcher _dispatcher;
        private readonly DispatcherPriority _priority;

        public DispatcherBeginInvokator()
            : this(Dispatcher.CurrentDispatcher, DispatcherPriority.Normal)
        {
        }

        public DispatcherBeginInvokator(Dispatcher dispatcher)
            : this(dispatcher, DispatcherPriority.Normal)
        {
        }

        public DispatcherBeginInvokator(DispatcherPriority priority)
            : this(Dispatcher.CurrentDispatcher, priority)
        {
        }

        public DispatcherBeginInvokator(Dispatcher dispatcher, DispatcherPriority priority)
        {
            _dispatcher = dispatcher;
            _priority = priority;
        }

        public void Invoke(Action action)
        {
            _dispatcher.BeginInvoke(action, _priority);
        }
    }
}