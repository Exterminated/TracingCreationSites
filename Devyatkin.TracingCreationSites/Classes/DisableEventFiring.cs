using Microsoft.SharePoint;
using System;

namespace Devyatkin.TracingCreationSites
{
    public sealed class EventReceiverScope : SPItemEventReceiver, IDisposable
    {
        private bool mOriginal = false;

        public EventReceiverScope(bool enabled)
        {
            mOriginal = EventFiringEnabled;
            EventFiringEnabled = enabled;
        }

        public void Dispose()
        {
            EventFiringEnabled = mOriginal;
        }
    }
}
