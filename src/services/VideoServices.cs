using System;
using System.IO;
using SimpleServer.Attributes;
using System.Collections.Generic;
using System.Diagnostics;
using HMStreamBackend.Dtos;

namespace HMStreamBackend.Services
{
    [Service("videoServices")]
    class VideoServices : IVideoServices
    {
        public static string VideoDirectory { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "HMStream");

        static VideoServices()
        {
            if (!Directory.Exists(VideoDirectory))
            {
                Directory.CreateDirectory(VideoDirectory);
            }
        }

        public byte[] GetBytes(string name, long from, long to)
        {
            System.Console.WriteLine($"{name} --- lower: {from} upper: {to}");
            throw new NotImplementedException();
        }

        public Video GetVideoByName(string name)
        {
            if (File.Exists(Path.Combine(VideoDirectory, name)))
            {
                FileInfo info = new FileInfo(Path.Combine(VideoDirectory, name));
                var toReturn = new Video(info.Length, name);
                return toReturn;
            }
            return null;
        }
    }
}