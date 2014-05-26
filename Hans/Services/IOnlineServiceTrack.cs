namespace Hans.Tests
{
    public interface IOnlineServiceTrack
    {
        string Artist { get; set; }
        string Title { get; set; }
        string Mp3Url { get; set; }
        string DisplayName { get;  }
        string GetFileName();
    }
}