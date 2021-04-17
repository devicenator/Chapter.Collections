using System;

namespace SniffCore.Collections
{
    public interface IInvokator
    {
        void Invoke(Action action);
    }
}