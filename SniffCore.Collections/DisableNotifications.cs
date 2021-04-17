using System;

namespace SniffCore.Collections
{
    internal class DisableNotifications : IDisposable
    {
        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Disposed;
    }
}