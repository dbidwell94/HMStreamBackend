using System;
using System.IO;
using SimpleServer.Attributes;
using HMStreamBackend.Dtos;
using HMStreamBackend.Exceptions;
using MediaToolkit;

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

        public Video GetVideoDetails(string name)
        {
            CheckVideoExists(name);
            using (var engine = new Engine("/usr/bin/ffmpeg"))
            {
                var inputFile = new MediaToolkit.Model.MediaFile(Path.Combine(VideoDirectory, name));
                engine.GetMetadata(inputFile);
                return new Video(new FileInfo(Path.Combine(VideoDirectory, name)), inputFile.Metadata);
            }
        }
    }
}