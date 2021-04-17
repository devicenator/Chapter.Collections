using System;
using System.Windows.Threading;

namespace SniffCore.Collections
{
    public sealed class DispatcherInvokator : IInvokator
    {
        private readonly Dispatcher _dispatcher;
        private readonly DispatcherPriority _priority;

        public DispatcherInvokator()
            : this(Dispatcher.CurrentDispatcher, DispatcherPriority.Normal)
        {
        }

        public DispatcherInvokator(Dispatcher dispatcher)
            : this(dispatcher, DispatcherPriority.Normal)
        {
        }

        public DispatcherInvokator(DispatcherPriority priority)
            : this(Dispatcher.CurrentDispatcher, priority)
        {
        }

        public DispatcherInvokator(Dispatcher dispatcher, DispatcherPriority priority)
        {
            _dispatcher = dispatcher;
            _priority = priority;
        }

        public void Invoke(Action action)
        {
            _dispatcher.Invoke(action, _priority);
        }
    }
}