using Hans.GeneralApp;

namespace Hans.General
{
    public class ExitTrigger
    {

        public void TriggerExit()
        {
            if (ExitTriggered != null)
            {
                ExitTriggered();
            }
        }

        public event ExitTriggeredEventHandler ExitTriggered;
    }
}