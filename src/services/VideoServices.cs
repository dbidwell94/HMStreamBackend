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

        public byte[] GetBytes(string name, long upper, long lower)
        {
            CheckVideoExists(name);
            var buffer = new byte[upper - lower];
            using (var reader = new FileStream(Path.Combine(VideoDirectory, name), FileMode.Open))
            {
                reader.Seek(lower, SeekOrigin.Begin);
                reader.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }

        public async Task<Video> GetVideoDetails(string name)
        {
            CheckVideoExists(name);
            var wrapper = new Wrapper();
            var fileInfo = new FileInfo(Path.Combine(VideoDirectory, name));
            var fileData = await wrapper.Input(fileInfo).GetInputInfo();
            return new Video(fileInfo, fileData);
        }
    }
}