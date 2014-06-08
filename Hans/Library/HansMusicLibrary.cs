using System;
using System.Collections.Generic;
using System.Linq;
using Hans.Database.Playlists;
using Hans.Database.Songs;
using Hans.SongData;

namespace Hans.Library
{
    public class HansMusicLibrary
    {
        private readonly IPlaylistStore _playlistStore;
        private ISongStore _songStore;
        public event NewLibrarySongEventHandler NewSong;

        protected virtual void OnNewSong(HansSong song)
        {
            var handler = NewSong;
            if (handler != null)
            {
                handler(song);
            }
        }

        /// <summary>
        /// Creates a new instance of the hans music library
        /// </summary>
        public HansMusicLibrary(IPlaylistStore playlistStore, ISongStore songStore, ISongDataFinder songDataFinder)
        {
            _playlistStore = playlistStore;
            _songStore = songStore;
            SongDataFinder = songDataFinder;



            SongDataFinder.FindAsync(new FindSongDataRequest
            {
                PathToFile = "D:\\Eigene Dateien\\Downloads\\PussycatDolls_Beep.mp3",
                SongId = Guid.NewGuid()
            });
        }

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

        public void AddSong(HansSong hansSong)
        {
            _songStore.Add(hansSong);
        }

        public void RemoveSong(HansSong hansSong)
        {
            _songStore.Remove(hansSong);
        }

        public void AddFolder(FolderAddRequest folderAddRequest)
        {
            var folderAnalyzer = new FolderAnalyzer();
            folderAnalyzer.FoundNewfile += folderAnalyzer_FoundNewfile;
            folderAnalyzer.StartAsync(new AnalyzeFolderRequest
            {
                Path = folderAddRequest.Path,
                FileExtensionFilter = new [] {"*.mp3"},
                ShouldAnalyzeSubDirectories = true
            });
            
        }

        void folderAnalyzer_FoundNewfile(string file)
        {
            var hansSong = new HansSong(file);
            SongDataFinder.FindAsync(new FindSongDataRequest
            {
                PathToFile = hansSong.FilePath,
                SongId = hansSong.Id
            });
            AddSong(hansSong);
        }

        private ISongDataFinder SongDataFinder { get; set; }

        public IEnumerable<HansSong> Search(string term)
        {
            return _songStore.GetEnumerable()
                .Where(s => s.Artist.ToLower().Contains(term) 
                || s.Title.ToLower().Contains(term) 
                || s.FilePath.ToLower().Contains(term)
                ).OrderBy(a => a.Title);
        }
    }


    public struct FolderAddRequest
    {
        public string Path { get; set; }
    }
}