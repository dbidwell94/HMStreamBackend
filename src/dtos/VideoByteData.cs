using System;

namespace HMStreamBackend.Dtos
{
    public class VideoByteData
    {
        public byte[] VideoData { get; private set; }
        public long DataSize { get; private set; }

        public VideoByteData(byte[] data, long size)
        {
            VideoData = data;
            DataSize = size;
        }
    }
}