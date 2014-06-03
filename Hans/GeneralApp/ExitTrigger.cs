namespace Hans.GeneralApp
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