using System;
using System.Collections.Generic;

namespace HMStreamBackend.Exceptions
{
    public static class ExceptionData
    {
        #nullable enable
        public static Dictionary<string, object> MakeExceptionData(string details, Exception thrownException)
        {
            #nullable disable
            var toReturn = new Dictionary<string, object>();
            toReturn.Add("timestamp", DateTime.Now);
            toReturn.Add("details", details);
            toReturn.Add("path", thrownException.TargetSite.Name);
            toReturn.Add("error", thrownException.Message);

            return toReturn;
        }
    }
}