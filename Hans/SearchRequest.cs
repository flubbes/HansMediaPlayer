﻿using Hans.Tests;

namespace Hans
{
    public struct SearchRequest
    {
        public string Query { get; set; }
        public IOnlineService OnlineService { get; set; }
    }
}