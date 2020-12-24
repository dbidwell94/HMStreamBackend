using System;
using System.Collections.Generic;
using SimpleServer.Attributes;
using SimpleServer.Networking.Data;

namespace HMStreamBackend.Controllers
{
    #nullable enable
    struct EndpointTest
    {
        public string? something;
        public IEnumerable<int>? someArr;

        public EndpointTest(string something, IEnumerable<int> someArr)
        {
            this.something = something;
            this.someArr = someArr;
        }
    }
    #nullable disable

    [RestController("/")]
    class TestController
    {
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
                    someArr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }
                };
            }
            return new ResponseEntity(testArr);
        }

        [GetMapping("/param-test", Produces = MediaTypes.ApplicationJson, Accepts = MediaTypes.ApplicationJson)]
        public ResponseEntity ParameterTest(EndpointTest test)
        {
            return new ResponseEntity(test);
        }

        [GetMapping("/param-test-arr", Produces = MediaTypes.ApplicationJson, Accepts = MediaTypes.ApplicationJson)]
        public ResponseEntity ParameterTestArray(EndpointTest[] testArr)
        {
            return new ResponseEntity(testArr);
        }
    }
}