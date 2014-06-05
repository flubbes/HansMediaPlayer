namespace Hans.General
{
    public interface ITrigger
    {
        void Trigger();
        event GotTriggeredEventHandler GotTriggered;
    }
}