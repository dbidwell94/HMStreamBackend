using System.IO;
using System;
using FFMpegWrapper.Models;
using FFMpegWrapper.Options;

namespace HMStreamBackend.Dtos
{
    public class Video
    {
        public long VideoSize { get; private set; }
        public string VideoName { get; private set; }
        public TimeSpan Duration { get; private set; }
        public VideoCodec VideoFormat { get; private set; }
        public AudioCodec AudioFormat { get; private set; }
        public double Fps { get; private set; }
        public string Bitrate { get; private set; }

        public Video(FileInfo videoFile, MediaFileInfo fileData)
        {
            VideoSize = videoFile.Length;
            VideoName = videoFile.Name;
            Duration = fileData.Duration;
            VideoFormat = fileData.VideoCodec;
            AudioFormat = fileData.AudioCodec;
            Fps = fileData.Fps;
            Bitrate = fileData.Bitrate;
        }
    }
}