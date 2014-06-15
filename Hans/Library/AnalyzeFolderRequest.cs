namespace Hans.Library
{
    /// <summary>
    /// An analyze folder request
    /// </summary>
    public struct AnalyzeFolderRequest
    {
        /// <summary>
        /// The extensions to filter for
        /// </summary>
        public string[] FileExtensionFilter { get; set; }

        /// <summary>
        /// The path to search in
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets if the folder analyzer should also analyze sub directories
        /// </summary>
        public bool ShouldAnalyzeSubDirectories { get; set; }
    }
}