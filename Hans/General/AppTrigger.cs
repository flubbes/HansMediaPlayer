using System;

namespace Hans.General
{
    /// <summary>
    /// Gives the possibility to trigger event when the app exits
    /// </summary>
    public abstract class AppTrigger
    {
        /// <summary>
        /// The event that gets triggered on exit
        /// </summary>
        public event GotTriggeredEventHandler GotTriggered;

        /// <summary>
        /// Triggers the event
        /// </summary>
        public void Trigger()
        {
            if (GotTriggered != null)
            {
                GotTriggered(this, EventArgs.Empty);
            }
        }
    }
}