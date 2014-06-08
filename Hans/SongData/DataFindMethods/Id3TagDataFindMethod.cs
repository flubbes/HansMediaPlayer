using Id3;
using Id3.Frames;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Hans.SongData.DataFindMethods
{
    public class Id3TagDataFindMethod : IDataFindMethod
    {
        public Dictionary<string, string> GetData(FindSongDataRequest request)
        {
            var dict = new Dictionary<string, string>();
            using (var fileStream = new FileStream(request.PathToFile, FileMode.Open))
            {
                ReadId3TagsFromFile(fileStream, dict);
            }
            return dict;
        }

        private static void AddToDictionary(Dictionary<string, string> dict, string value, string key)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, value);
                }
            }
        }

        private static IEnumerable<PropertyInfo> GetTextFramePropertyInfos(Id3Tag tag)
        {
            return tag.GetType().GetProperties().Where(p => p.PropertyType.BaseType == typeof(TextFrame));
        }

        private void HandleId3TagVersion(Id3Tag tag, Dictionary<string, string> dict)
        {
            foreach (var propertyInfo in GetTextFramePropertyInfos(tag))
            {
                PropertyInfo info = propertyInfo;
                HandleProperty(() => info.GetValue(tag) as TextFrame, dict);
            }
        }

        private void HandleId3Versions(Mp3Stream file, Dictionary<string, string> dict)
        {
            foreach (var tag in file.GetAllTags())
            {
                HandleId3TagVersion(tag, dict);
            }
        }

        private void HandleProperty(Func<TextFrame> func, Dictionary<string, string> dict)
        {
            var textFrame = func.Invoke();
            var key = textFrame.GetType().Name.Replace("Frame", "");
            var value = textFrame.Value;
            AddToDictionary(dict, value, key);
        }

        private void ReadId3TagsFromFile(FileStream fileStream, Dictionary<string, string> dict)
        {
            var file = new Mp3Stream(fileStream);
            if (!file.HasTags)
            {
                return;
            }
            HandleId3Versions(file, dict);
        }
    }
}