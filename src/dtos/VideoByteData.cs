using System;

namespace HMStreamBackend.Dtos
{
    public class VideoByteData
    {
        public byte[] VideoData { get; private set; }
        public long DataSize { get; private set; }
        public long RemainingBytes{ get; private set; }

        public VideoByteData(byte[] data, long remaining)
        {
            VideoData = data;
            DataSize = data.Length;
            RemainingBytes = remaining;
        }
    }
}