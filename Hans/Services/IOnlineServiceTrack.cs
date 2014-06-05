﻿using Hans.Web;
using YoutubeExtractor;

namespace Hans.Services
{
    public interface IOnlineServiceTrack
    {
        string Artist { get; set; }
        string Title { get; set; }
        string Mp3Url { get; set; }
        string DisplayName { get;  }
        string GetFileName();
        IDownloader GetDownloader();
    }
}