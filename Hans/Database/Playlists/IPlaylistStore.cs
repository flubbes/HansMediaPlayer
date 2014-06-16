namespace Hans.Database.Playlists
{
    /// <summary>
    /// Needed class for the ninject dependency injection
    /// </summary>
    public interface IPlaylistStore : IStore<HansPlaylist>
    {
    }
}