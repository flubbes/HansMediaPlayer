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

        private static IEnumerable<PropertyInfo> GetFramePropertyInfos(Id3Tag tag, Type type)
        {
            return tag.GetType().GetProperties().Where(p => p.PropertyType.BaseType == type);
        }

        private void HandleId3TagVersion(Id3Tag tag, Dictionary<string, string> dict)
        {
            foreach (var textFrame in GetFramePropertyInfos(tag, typeof(TextFrame)))
            {
                var info = textFrame;
                HandleTextFrame(() => info.GetValue(tag) as TextFrame, dict);
            }
            foreach (var listTextFrame in GetFramePropertyInfos(tag, typeof(ListTextFrame)))
            {
                var info = listTextFrame;
                HandleTextFrame(() => info.GetValue(tag) as TextFrame, dict);
            }
        }

        private void HandleId3Versions(Mp3Stream file, Dictionary<string, string> dict)
        {
            try
            {
                foreach (var tag in file.GetAllTags())
                {
                    HandleId3TagVersion(tag, dict);
                }
            }
            catch { }
        }

        private void HandleTextFrame(Func<TextFrame> func, Dictionary<string, string> dict)
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