using System.Windows;

namespace Hans.General
{
    public class AppTrigger
    {

        public void Trigger()
        {
            if (GotTriggered != null)
            {
                GotTriggered();
            }
        }

        public event GotTriggeredEventHandler GotTriggered;
    }
}