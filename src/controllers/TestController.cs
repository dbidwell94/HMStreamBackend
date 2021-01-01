using System.Net;
using SimpleServer.Attributes;
using SimpleServer.Networking.Data;
using SimpleServer.Networking;
using SimpleServer.Networking.Headers;
using HMStreamBackend.Services;

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

        [GetMapping("/video/:videoName/bytes", Produces = MediaTypes.ApplicationJson, Accepts = MediaTypes.ApplicationJson)]
        public byte[] GetVideoBytes([PathParam] string videoName, [Injected] WebHeaderCollection headers)
        {
            var range = helperFunctions.RangeHeaderToBytes(headers.Get("Range"));
            return videoServices.GetBytes(videoName, range.lower, range.upper);
        }
    }
}