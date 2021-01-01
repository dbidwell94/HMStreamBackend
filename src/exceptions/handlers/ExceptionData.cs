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
            toReturn.Add("message", thrownException.Message);
            toReturn.Add("details", details);
            toReturn.Add("path", thrownException.TargetSite.Name);
            toReturn.Add("errors", GetInnerException(thrownException));
            toReturn.Add("developerInfo", GetStackTrace(thrownException));
            return toReturn;
        }

        private static string GetInnerException(Exception thrownException)
        {
            var toReturn = new List<string>();
            while (thrownException != null)
            {
                toReturn.Add(thrownException.Message);
                thrownException = thrownException.InnerException;
            }
            return toReturn.ToArray()[toReturn.Count - 1];
        }

        private static string[] GetStackTrace(Exception thrownException)
        {
            var toParse = thrownException.ToString();
            var toReturn = toParse.Split('\n');
            return toReturn;
        }
    }
}