using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hans.Core.Services;
using Newtonsoft.Json;

namespace Hans.Core.Database.Songs
{
    /// <summary>
    /// The song class from hans
    /// </summary>
    public class HansSong
    {
        private List<string> _artists;
        private string _title;

        /// <summary>
        /// Initializes a new hans song
        /// </summary>
        /// <param name="filePath"></param>
        public HansSong(string filePath)
        {
            Id = Guid.NewGuid();
            FilePath = filePath;
            _artists = new List<string>();
            Title = string.Empty;
        }

        /// <summary>
        /// The first artist from the song
        /// </summary>
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

        /// <summary>
        /// All artists from the song
        /// </summary>
        public IEnumerable<string> Artists
        {
            get { return _artists; }
            set { _artists = (List<string>)value; }
        }

        /// <summary>
        /// The filepath
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// The genre
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// The id of the song
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The title of the song
        /// </summary>
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

        /// <summary>
        /// Factory method to create a hanssong from an online service track
        /// </summary>
        /// <param name="path"></param>
        /// <param name="track"></param>
        /// <returns></returns>
        public static HansSong FromOnlineServiceTrack(string path, IOnlineServiceTrack track)
        {
            return new HansSong(path)
            {
                _artists = new List<string>(new[] { track.Artist }),
                Title = track.Title
            };
        }

        /// <summary>
        /// adds an artist to the artists
        /// </summary>
        /// <param name="artist"></param>
        public void AddArtist(string artist)
        {
            _artists.Add(artist);
        }

        /// <summary>
        /// Adds multiple artists to the artists of this song
        /// </summary>
        /// <param name="artists"></param>
        public void AddArtists(IEnumerable<string> artists)
        {
            _artists.AddRange(artists);
        }
    }
}