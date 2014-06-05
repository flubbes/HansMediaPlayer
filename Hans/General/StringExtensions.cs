using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hans.General
{
    public static class StringExtensions
    {
        public static string RemoveIllegalCharacters(this string s)
        {
            var newName = string.Empty;
            var invalidCharacters = new List<char>(Path.GetInvalidFileNameChars());
            invalidCharacters.AddRange(Path.GetInvalidPathChars());
            return s.Where(c => !invalidCharacters.Contains(c)).Aggregate(newName, (current, c) => current + c);
        }
    }
}
