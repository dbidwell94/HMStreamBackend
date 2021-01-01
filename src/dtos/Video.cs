namespace HMStreamBackend.Dtos
{
    public class Video
    {
        public long VideoSize { get; private set; }
        public string VideoName { get; private set; }

        public Video(long size, string name)
        {
            VideoSize = size;
            VideoName = name;
        }
    }
}