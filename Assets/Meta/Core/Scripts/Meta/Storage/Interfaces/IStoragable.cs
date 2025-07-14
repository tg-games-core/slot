namespace Core
{
    public interface IStoragable
    {
        public string Key { get; }
        public StorageData StorageData { get; set; } 
    }
}
