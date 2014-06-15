using Hans.Database.Playlists;
using Hans.Database.Songs;
using Hans.General;
using Hans.SongData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Hans.Library
{
    /// <summary>
    /// The hans music library
    /// </summary>
    public sealed class HansMusicLibrary
    {
        private readonly IPlaylistStore _playlistStore;
        private ISongStore _songStore;

        /// <summary>
        /// Creates a new instance of the hans music library
        /// </summary>
        public HansMusicLibrary(IPlaylistStore playlistStore, ISongStore songStore, ISongDataFinder songDataFinder)
        {
            _playlistStore = playlistStore;
            _songStore = songStore;
            SongDataFinder = songDataFinder;
            songDataFinder.FoundData += songDataFinder_FoundData;
        }

        /// <summary>
        /// When a new song is added to the library
        /// </summary>
        public event NewLibrarySongEventHandler NewSong;

        /// <summary>
        /// When a search is finished in the local library
        /// </summary>
        public event LibrarySearchFinishedEventHandler SearchFinished;

        /// <summary>
        /// Alls playlists in this library
        /// </summary>
        public IEnumerable<HansPlaylist> Playlists
        {
            get
            {
                return _playlistStore.GetEnumerable();
            }
        }

        /// <summary>
        /// All the songs in the library
        /// </summary>
        public IEnumerable<HansSong> Songs
        {
            get
            {
                return _songStore.GetEnumerable();
            }
        }

        /// <summary>
        /// The song data finder isntance
        /// </summary>
        private ISongDataFinder SongDataFinder { get; set; }

        /// <summary>
        /// Adds a folder to the library
        /// </summary>
        /// <param name="folderAddRequest"></param>
        public void AddFolder(FolderAddRequest folderAddRequest)
        {
            var folderAnalyzer = new FolderAnalyzer();
            folderAnalyzer.FoundNewfile += folderAnalyzer_FoundNewfile;
            folderAnalyzer.StartAsync(new AnalyzeFolderRequest
            {
                Path = folderAddRequest.Path,
                FileExtensionFilter = new[] { "*.mp3" },
                ShouldAnalyzeSubDirectories = true
            });
        }

        /// <summary>
        /// Adds a new song the library
        /// </summary>
        /// <param name="hansSong"></param>
        public void AddSong(HansSong hansSong)
        {
            _songStore.Add(hansSong);
            OnNewSong(hansSong);
        }

        /// <summary>
        /// Creates a new playlist
        /// </summary>
        /// <param name="name"></param>
        public void CreatePlayList(string name)
        {
            _playlistStore.Add(new HansPlaylist
            {
                Id = Guid.NewGuid(),
                Name = name
            });
        }

        /// <summary>
        /// removes a song from the library
        /// </summary>
        /// <param name="hansSong"></param>
        public void RemoveSong(HansSong hansSong)
        {
            _songStore.Remove(hansSong);
        }

        /// <summary>
        /// Searches for a term in the library and calls the Searchfinished event at the end
        /// </summary>
        /// <param name="term"></param>
        public void Search(string term)
        {
            var t = term.ToLower();
            new Thread(() => OnSearchFinished(new LibrarySearchFinishedEventArgs
            {
                Tracks = _songStore.GetEnumerable()
                    .Where(s => s.Artists.Any(a => a.ToLower().Contains(t))
                                || s.Title.ToLower().Contains(t)
                                || s.FilePath.ToLower().Contains(t)
                    ).OrderBy(a => a.Title).BuildThreadSafeCopy()
            })).Start();
        }

        /// <summary>
        /// If the song id exists in the library
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool Exists(Guid id)
        {
            return _songStore.GetEnumerable().Any(s => s.Id == id);
        }

        /// <summary>
        /// Gets called when the folder Analyzer found a new file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void folderAnalyzer_FoundNewfile(object sender, FoundNewFileEventArgs e)
        {
            var hansSong = new HansSong(e.File);
            SongDataFinder.FindAsync(new FindSongDataRequest
            {
                PathToFile = hansSong.FilePath,
                SongId = hansSong.Id
            });
        }

        /// <summary>
        /// Triggers the new song event
        /// </summary>
        /// <param name="song"></param>
        private void OnNewSong(HansSong song)
        {
            var handler = NewSong;
            if (handler != null)
            {
                handler(this, new NewLibrarySongEventArgs { Song = song });
            }
        }

        /// <summary>
        /// Triggers the search finished event
        /// </summary>
        /// <param name="e"></param>
        private void OnSearchFinished(LibrarySearchFinishedEventArgs e)
        {
            var handler = SearchFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Gets called when the song data finder found new data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void songDataFinder_FoundData(object sender, FoundDataEventArgs e)
        {
            var songData = e.SongData;
            if (Exists(songData.SongId))
            {
                var hansSong = _songStore.GetEnumerable().Single(s => s.Id == songData.SongId);
                hansSong.AddArtists(songData.Artists);
                hansSong.Title = songData.Title;
                hansSong.Genre = songData.Genre;
            }
            else
            {
                var hansSong = new HansSong(songData.FilePath)
                {
                    Artists = songData.Artists,
                    Genre = songData.Genre,
                    Title = songData.Title
                };
                AddSong(hansSong);
            }
        }
    }
}