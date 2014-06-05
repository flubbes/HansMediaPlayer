using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

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
