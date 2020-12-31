using System;
using SimpleServer.Attributes;
using System.Collections.Generic;

namespace HMStreamBackend.Services
{
    [Service("videoServices")]
    class VideoServices
    {
        public Dictionary<string, object> GetVideoName()
        {
            return new Dictionary<string, object>() { { "videoName", "Look at this awesome video name!" } };
        }
    }
}