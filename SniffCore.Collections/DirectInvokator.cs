using System;

namespace SniffCore.Collections
{
    public sealed class DirectInvokator : IInvokator
    {
        public void Invoke(Action action)
        {
            action();
        }
    }
}