using System;
using System.Net;
using SimpleServer.Attributes;
using SimpleServer.Networking.Data;
using SimpleServer.Networking;
using SimpleServer.Networking.Headers;
using HMStreamBackend.Services;
using HMStreamBackend.Dtos;

namespace HMStreamBackend.Controllers
{
    [RestController("/")]
    class TestController
    {
        public static ServerResponseHeaders Headers { get; private set; } = new ServerResponseHeaders();

        [Autowired]
        public VideoServices videoServices { get; private set; }

        [Autowired]
        public HelperFunctions helperFunctions { get; private set; }


        public TestController()
        {
            Headers.SetCors(CorsHeader.BuildHeader("*"));
        }

        [GetMapping("/video/:videoName/size", Produces = MediaTypes.ApplicationJson, Accepts = MediaTypes.ApplicationJson)]
        public ResponseEntity GetVideoByName([PathParam] string videoName)
        {
            return new ResponseEntity(videoServices.GetVideoByName(videoName), Headers);
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
            byte[] videoData = videoServices.GetBytes(videoName, range.upper, range.lower);
            VideoByteData data = new VideoByteData(videoData, videoData.Length);
            return new ResponseEntity(data, Headers);
        }
    }
}