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
#nullable enable
    struct EndpointTest
    {
        public string? something;
        public IEnumerable<int>? someArr;

        public string requiredProp;

        public EndpointTest(string something, IEnumerable<int> someArr, string requiredProp)
        {
            this.something = something;
            this.someArr = someArr;
            this.requiredProp = requiredProp;
        }
    }
#nullable disable

    [RestController("/")]
    class TestController
    {
        [Autowired]
        public VideoServices videoServices { get; private set; }


        [GetMapping("/", Produces = MediaTypes.ApplicationJson, Accepts = MediaTypes.ApplicationJson)]
        public ResponseEntity TestMessage()
        {
            return new ResponseEntity("test");
        }

        [GetMapping("/anothertest", Produces = MediaTypes.ApplicationJson, Accepts = MediaTypes.ApplicationJson)]
        public ResponseEntity AnotherTestMessage()
        {
            int amount = 50;
            EndpointTest[] testArr = new EndpointTest[amount];
            for (int i = 0; i < amount; i++)
            {
                testArr[i] = new EndpointTest
                {
                    something = $"This is a test to see if {i} works",
                    someArr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 },
                    requiredProp = "This is a required property"
                };
            }
            return new ResponseEntity(testArr);
        }

        [GetMapping("/video/:videoName", Produces = MediaTypes.ApplicationJson, Accepts = MediaTypes.ApplicationJson)]
        public ResponseEntity PathParameterTest([PathParam] string videoName)
        {
            Dictionary<string, object> toReturn = new Dictionary<string, object>();
            toReturn.Add("videoName", videoName);

            var headers = new ServerResponseHeaders();
            headers.SetCors(CorsHeader.BuildHeader("*"));

            return new ResponseEntity(toReturn, headers);
        }

        [GetMapping("/copytimes/:amount", Produces = MediaTypes.ApplicationJson, Accepts = MediaTypes.ApplicationJson)]
        public ResponseEntity FullParameterTest([RequestBody] EndpointTest? endpointTest, [PathParam] int amount)
        {
            EndpointTest[] toReturn;
            if (endpointTest.HasValue)
            {
                toReturn = new EndpointTest[amount];
                for (int i = 0; i < amount; i++)
                {
                    toReturn[i] = endpointTest.Value;
                }
            }
            else
            {
                toReturn = null;
            }
            Dictionary<string, object> returnObj = new Dictionary<string, object>();
            returnObj.Add("endpointTest", toReturn);

            ServerResponseHeaders headers = new ServerResponseHeaders();
            headers.SetCors(CorsHeader.BuildHeader("*"));

            return new ResponseEntity(returnObj, headers);
        }

        [GetMapping("/videoname", Accepts = MediaTypes.ApplicationJson, Produces = MediaTypes.ApplicationJson)]
        public ResponseEntity GetVideoName()
        {
            ServerResponseHeaders headers = new ServerResponseHeaders();
            return new ResponseEntity(videoServices.GetVideoName(), headers);
        }
    }
}