using SimpleServer.Exceptions;
using SimpleServer.Networking.Data;

namespace HMStreamBackend.Exceptions
{
    public class EntityNotFoundException : AbstractServerException
    {
        public EntityNotFoundException(string message) : base(message, HttpStatus.NOT_FOUND)
        {
        }
    }
}