namespace Hans.General
{
    public abstract class AppTrigger
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