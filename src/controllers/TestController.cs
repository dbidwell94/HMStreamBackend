using System.Net.Http.Headers;
using System.Security.Authentication.ExtendedProtection;
using System;
using System.Collections.Generic;
using SimpleServer.Attributes;
using SimpleServer.Networking;
using SimpleServer.Networking.Data;
using SimpleServer.Networking.Headers;
using HMStreamBackend.Services;

namespace HMStreamBackend.Controllers
{
    [RestController("/")]
    class TestController
    {

        [Autowired]
        public VideoServices videoServices{ get; private set; }

        [GetMapping("/video/:videoName", Produces = MediaTypes.ApplicationJson, Accepts = MediaTypes.ApplicationJson)]
        public ResponseEntity GetVideoBytes([PathParam] string videoName, [Injected] HttpRequestHeaders requestHeaders)
        {
            return new ResponseEntity(new object[] { videoServices.GetVideoName(), requestHeaders.Range.Ranges });
        }
    }
}