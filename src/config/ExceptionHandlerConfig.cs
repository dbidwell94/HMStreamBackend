using SimpleServer.Exceptions;
using SimpleServer.Attributes;
using SimpleServer.Networking.Data;
using HMStreamBackend.Exceptions;

namespace HMStreamBackend.Config
{
    [Config]
    public class ExceptionHandler : IServerExceptionHandler
    {
        public ResponseEntity HandleEndpointNotValidException(ServerEndpointNotValidException exception)
        {
            var toReturn = ExceptionData.MakeExceptionData("REST Endpoint not valid", exception);
            return new ResponseEntity(toReturn);
        }

        public ResponseEntity HandleServerRequestMethodNotSupportedException(ServerRequestMethodNotSupportedException exception)
        {
            var toReturn = ExceptionData.MakeExceptionData("HTTP Method not supported", exception);
            return new ResponseEntity(toReturn);
        }

        public ResponseEntity HandleAbstractServerException(AbstractServerException exception)
        {
            var toReturn = ExceptionData.MakeExceptionData("No valid error handling available", exception);
            return new ResponseEntity(toReturn);
        }

        public ResponseEntity HandleInternalServerErrorException(InternalServerErrorException exception)
        {
            var toReturn = ExceptionData.MakeExceptionData("An internal server error has occured", exception);
            return new ResponseEntity(toReturn);
        }

    }
}