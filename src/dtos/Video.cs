using System.IO;
using System;
using MediaToolkit.Model;


namespace HMStreamBackend.Dtos
{
    public class Video
    {
        public long VideoSize { get; private set; }
        public string VideoName { get; private set; }
        public TimeSpan Duration { get; private set; }
        public string VideoFormat { get; private set; }
        public string AudioFormat { get; private set; }
        public double Fps { get; private set; }

        public Video(FileInfo videoFile, Metadata fileData)
        {
            VideoSize = videoFile.Length;
            VideoName = videoFile.Name;
            Duration = fileData.Duration;
            VideoFormat = fileData.VideoData.Format;
            AudioFormat = fileData.AudioData.Format;
            Fps = fileData.VideoData.Fps;
        }
    }
}