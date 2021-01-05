using System.Collections.Generic;
using System;
using System.IO;
using System.Threading.Tasks;
using SimpleServer.Attributes;
using HMStreamBackend.Dtos;
using HMStreamBackend.Exceptions;
using FFMpegWrapper;

namespace HMStreamBackend.Services
{
    [Service("videoServices")]
    class VideoServices : IVideoServices
    {
        public static string VideoDirectory { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "HMStream");
        public static readonly HashSet<string> ValidVideoExtensions = new HashSet<string>() { ".webm", ".mp4", ".avi", ".mov" };

        private void CheckVideoExists(string name)
        {
            if (!File.Exists(Path.Combine(VideoDirectory, name)))
            {
                throw new EntityNotFoundException($"Video {name} does not exist");
            }
        }

        static VideoServices()
        {
            if (!Directory.Exists(VideoDirectory))
            {
                Directory.CreateDirectory(VideoDirectory);
            }
        }

        public (byte[] videoData, long totalSize, long upper, long lower) GetBytes(string name, long upper, long lower)
        {
            CheckVideoExists(name);
            FileInfo info = new FileInfo(Path.Combine(VideoDirectory, name));

            // Some sanity checks here
            if (upper > info.Length)
            {
                upper = info.Length;
            }
            if (lower < 0)
            {
                throw new Exception("Lower range cannot be lower than 0");
            }
            if (upper < lower)
            {
                throw new Exception("Upper range cannot be lower than lower range");
            }

            var buffer = new byte[upper - lower];
            long size = info.Length;
            using (var reader = new FileStream(info.FullName, FileMode.Open))
            {
                reader.Seek(lower, SeekOrigin.Begin);
                reader.Read(buffer, 0, buffer.Length);
            }
            return (videoData: buffer, totalSize: size, upper: upper, lower: lower);
        }

        public async Task<Video> GetVideoDetails(string name)
        {
            CheckVideoExists(name);
            var wrapper = new Wrapper();
            var fileInfo = new FileInfo(Path.Combine(VideoDirectory, name));
            var fileData = await wrapper.Input(fileInfo).GetInputInfo();
            return new Video(fileInfo, fileData);
        }

        public async Task<Video[]> GetVideoLibrary()
        {
            var videos = new List<Video>();
            foreach (var file in Directory.GetFiles(VideoDirectory))
            {
                var info = new FileInfo(file);
                System.Console.WriteLine(info.Extension);
                if (ValidVideoExtensions.Contains(info.Extension))
                {
                    videos.Add(await GetVideoDetails(file));
                }
            }
            return videos.ToArray();
        }
    }
}