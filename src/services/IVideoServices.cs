using HMStreamBackend.Dtos;

namespace HMStreamBackend.Services
{
    public interface IVideoServices
    {
        byte[] GetBytes(string name, long from, long to);
        public Video GetVideoDetails(string name);


    }
}