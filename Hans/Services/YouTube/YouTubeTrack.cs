using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hans.Tests;

namespace Hans.Services.YouTube
{
    class YouTubeTrack : IOnlineServiceTrack
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Mp3Url { get; set; }
        public string DisplayName { get; private set; }
        public string GetFileName()
        {
            throw new NotImplementedException();
        }
    }
}
