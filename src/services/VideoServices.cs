using System;
using System.IO;
using SimpleServer.Attributes;
using HMStreamBackend.Dtos;
using HMStreamBackend.Exceptions;
using SimpleServer.Networking.Data;

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

        public byte[] GetBytes(string name, long upper, long lower)
        {
            if (File.Exists(Path.Combine(VideoDirectory, name)))
            {
                var buffer = new byte[upper - lower];
                using (var reader = new FileStream(Path.Combine(VideoDirectory, name), FileMode.Open))
                {
                    reader.Seek(lower, SeekOrigin.Begin);
                    reader.Read(buffer, 0, buffer.Length);
                }
                return buffer;
            }
            throw new EntityNotFoundException($"Video {name} does not exist");
        }

        public Video GetVideoByName(string name)
        {
            if (File.Exists(Path.Combine(VideoDirectory, name)))
            {
                FileInfo info = new FileInfo(Path.Combine(VideoDirectory, name));
                var toReturn = new Video(info.Length, name);
                return toReturn;
            }
            throw new EntityNotFoundException($"Video {name} does not exist");
        }
    }
}