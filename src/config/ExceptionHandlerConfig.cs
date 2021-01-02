using SimpleServer.Exceptions;
using SimpleServer.Attributes;
using SimpleServer.Networking.Data;
using SimpleServer.Networking;
using SimpleServer.Networking.Headers;
using HMStreamBackend.Exceptions;

namespace HMStreamBackend.Config
{
    [Config]
    public class ExceptionHandler : IServerExceptionHandler
    {

        public static ServerResponseHeaders Headers {get; private set;}

        static ExceptionHandler()
        {
            Headers = new ServerResponseHeaders();
            Headers.SetCors(CorsHeader.BuildHeader("*"));
        }

        public ResponseEntity HandleEndpointNotValidException(ServerEndpointNotValidException exception)
        {
            var toReturn = ExceptionData.MakeExceptionData("REST Endpoint not valid", exception);
            return new ResponseEntity(toReturn, Headers, HttpStatus.NOT_FOUND);
        }

        public ResponseEntity HandleServerRequestMethodNotSupportedException(ServerRequestMethodNotSupportedException exception)
        {
            var toReturn = ExceptionData.MakeExceptionData("HTTP Method not supported", exception);
            return new ResponseEntity(toReturn, Headers, exception.Status);
        }

        public ResponseEntity HandleAbstractServerException(AbstractServerException exception)
        {
            var toReturn = ExceptionData.MakeExceptionData("No valid error handling available", exception);
            var entity = new ResponseEntity(toReturn, Headers, exception.Status);
            return entity;
        }

        public ResponseEntity HandleInternalServerErrorException(InternalServerErrorException exception)
        {
            var toReturn = ExceptionData.MakeExceptionData("An internal server error has occured", exception);
            var entity = new ResponseEntity(toReturn, Headers, exception.Status);
            return entity;
        }

    }
}