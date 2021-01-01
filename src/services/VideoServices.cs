using System;
using System.IO;
using SimpleServer.Attributes;
using System.Collections.Generic;
using System.Diagnostics;

namespace HMStreamBackend.Services
{
    [Service("videoServices")]
    class VideoServices : IVideoServices
    {
        public static string VideoDirectory { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "HMStream");

        public byte[] GetBytes(long from, long to)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> GetVideoName()
        {
            return new Dictionary<string, object>() { { "videoName", "Look at this awesome video name!" } };
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}