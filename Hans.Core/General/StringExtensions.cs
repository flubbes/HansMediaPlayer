using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hans.Core.General
{
    /// <summary>
    /// String extension methods
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Removes illegal characters from the string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RemoveIllegalCharacters(this string s)
        {
            var newName = string.Empty;
            var invalidCharacters = new List<char>(Path.GetInvalidFileNameChars());
            invalidCharacters.AddRange(Path.GetInvalidPathChars());
            return s.Where(c => !invalidCharacters.Contains(c)).Aggregate(newName, (current, c) => current + c);
        }
    }
}