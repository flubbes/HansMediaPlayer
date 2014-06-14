using System;

namespace Hans.General
{
    public abstract class AppTrigger
    {
        public event GotTriggeredEventHandler GotTriggered;

        public void Trigger()
        {
            if (GotTriggered != null)
            {
                GotTriggered(this, EventArgs.Empty);
            }
        }
    }
}