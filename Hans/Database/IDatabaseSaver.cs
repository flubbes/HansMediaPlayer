namespace Hans.Database
{
    public interface IDatabaseSaver
    {
        void Save<T>(T toSave, string filePath);
    }
}