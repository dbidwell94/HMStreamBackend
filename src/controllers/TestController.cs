using System;
using SimpleServer.Attributes;
using SimpleServer.Networking.Data;

namespace HMStreamBackend.Controllers
{
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
            return new ResponseEntity("Another test");
            
        }
    }
}