using SimpleServer.Attributes;
using System.Text.RegularExpressions;

namespace HMStreamBackend.Services
{
    [Service("helperFunctions")]
    public class HelperFunctions
    {
        public (long lower, long upper) RangeHeaderToBytes(string rangeHeader)
        {
            var parseRegex = new Regex(@"(\d+)-(\d+)");
            var match = parseRegex.Match(rangeHeader);
            if (match.Groups.Count >= 3 && long.TryParse(match.Groups[1].Value, out long lower) && long.TryParse(match.Groups[2].Value, out long upper))
            {
                return (lower, upper);
            }
            return (0, 0);
        }
    }
}