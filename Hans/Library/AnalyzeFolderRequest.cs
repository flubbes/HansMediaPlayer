namespace Hans.Library
{
    public struct AnalyzeFolderRequest
    {
        public string Path { get; set; }

        public bool ShouldAnalyzeSubDirectories { get; set; }
        public string[] FileExtensionFilter { get; set; }
    }
}