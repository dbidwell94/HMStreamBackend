using System;
using System.Net;
using System.Threading.Tasks;
using SimpleServer.Attributes;
using SimpleServer.Networking.Data;
using SimpleServer.Networking;
using SimpleServer.Networking.Headers;
using HMStreamBackend.Services;
using HMStreamBackend.Dtos;

namespace HMStreamBackend.Controllers
{
    [RestController("/")]
    class VideoController
    {
        public static ServerResponseHeaders Headers { get; private set; } = new ServerResponseHeaders();

        [Autowired]
        public VideoServices videoServices { get; private set; }

        [Autowired]
        public HelperFunctions helperFunctions { get; private set; }


        public VideoController()
        {
            Headers.SetCors(CorsHeader.BuildHeader("*"));
        }

        [AllowHeaders("Range,Allow")]
        [GetMapping("/video/:videoName/bytes", Produces = MediaTypes.ApplicationJson, Accepts = MediaTypes.ApplicationJson)]
        public ResponseEntity GetVideoBytes([PathParam] string videoName, [Injected] WebHeaderCollection headers)
        {
            if (headers.Get("Range") == null)
            {
                throw new Exception("No range header present");
            }
            var range = helperFunctions.RangeHeaderToBytes(headers.Get("Range"));
            var videoData = videoServices.GetBytes(videoName, range.upper, range.lower);
            var remaining = videoData.totalSize - videoData.upper;
            VideoByteData data = new VideoByteData(videoData.videoData, remaining);
            return new ResponseEntity(data, Headers);
        }

        [GetMapping("/video/:videoName", Accepts = MediaTypes.ApplicationJson, Produces = MediaTypes.ApplicationJson)]
        public async Task<ResponseEntity> GetVideoData([PathParam] string videoName)
        {
            var details = await videoServices.GetVideoDetails(videoName);
            return new ResponseEntity(details, Headers);
        }

        [GetMapping("/videos", Accepts = MediaTypes.ApplicationJson, Produces = MediaTypes.ApplicationJson)]
        public async Task<ResponseEntity> GetVideoLibrary()
        {
            var videos = await videoServices.GetVideoLibrary();
            return new ResponseEntity(videos, Headers);
        }
    }
}