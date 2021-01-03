using HMStreamBackend.Dtos;
using System.Threading.Tasks;

namespace HMStreamBackend.Services
{
    public interface IVideoServices
    {
        byte[] GetBytes(string name, long from, long to);
        Task<Video> GetVideoDetails(string name);
    }
}