namespace HMStreamBackend.Services
{
    public interface IVideoServices
    {
        byte[] GetBytes(long from, long to);
    }
}