using Hans.Database.Playlists;
using Hans.Database.Songs;
using Hans.General;
using Hans.SongData;
using Ninject.Infrastructure.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Hans.Library
{
    public class HansMusicLibrary
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

        public event NewLibrarySongEventHandler NewSong;

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

        public IEnumerable<HansSong> Songs
        {
            get
            {
                //TODO update to new format
                return _songStore.GetEnumerable();
            }
        }

        private ISongDataFinder SongDataFinder { get; set; }

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

        public void RemoveSong(HansSong hansSong)
        {
            _songStore.Remove(hansSong);
        }

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

        protected virtual void OnNewSong(HansSong song)
        {
            var handler = NewSong;
            if (handler != null)
            {
                handler(this, new NewLibrarySongEventArgs { Song = song });
            }
        }

        protected virtual void OnSearchFinished(LibrarySearchFinishedEventArgs e)
        {
            var handler = SearchFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private bool Exists(Guid id)
        {
            return _songStore.GetEnumerable().Any(s => s.Id == id);
        }

        private void folderAnalyzer_FoundNewfile(object sender, FoundNewFileEventArgs e)
        {
            var hansSong = new HansSong(e.File);
            SongDataFinder.FindAsync(new FindSongDataRequest
            {
                PathToFile = hansSong.FilePath,
                SongId = hansSong.Id
            });
        }

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