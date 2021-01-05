using HMStreamBackend.Dtos;
using System.Threading.Tasks;

namespace HMStreamBackend.Services
{
    public interface IVideoServices
    {
        (byte[] videoData, long totalSize, long upper, long lower) GetBytes(string name, long from, long to);
        Task<Video> GetVideoDetails(string name);
        Task<Video[]> GetVideoLibrary();
    }
}