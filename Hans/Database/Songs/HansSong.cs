using Hans.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hans.Database.Songs
{
    public class HansSong
    {
        private List<string> _artists;
        private string _title;

        public HansSong(string filePath)
        {
            Id = Guid.NewGuid();
            FilePath = filePath;
            _artists = new List<string>();
            Title = string.Empty;
        }

        [JsonIgnore]
        public string Artist
        {
            get
            {
                if (!_artists.Any())
                {
                    return Path.GetFileNameWithoutExtension(FilePath);
                }
                return _artists.FirstOrDefault();
            }
        }

        public IEnumerable<string> Artists
        {
            get { return _artists; }
            set { _artists = (List<string>)value; }
        }

        public string FilePath { get; set; }

        public string Genre { get; set; }

        public Guid Id { get; set; }

        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_title))
                {
                    return Path.GetFileNameWithoutExtension(FilePath);
                }
                return _title;
            }
            set { _title = value; }
        }

        public static HansSong FromOnlineServiceTrack(string path, IOnlineServiceTrack track)
        {
            return new HansSong(path)
            {
                _artists = new List<string>(new[] { track.Artist }),
                Title = track.Title
            };
        }

        public void AddArtist(string artist)
        {
            _artists.Add(artist);
        }

        public void AddArtists(IEnumerable<string> artists)
        {
            _artists.AddRange(artists);
        }
    }
}