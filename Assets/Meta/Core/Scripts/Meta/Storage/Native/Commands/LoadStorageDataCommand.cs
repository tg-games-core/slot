namespace Core
{
    public class LoadStorageDataCommand : AbstractCommand
    {
        private readonly Storage _storage;
        private readonly IStoragable _storagable;
        
        public LoadStorageDataCommand(Storage storage, IStoragable storagable)
        {
            _storage = storage;
            _storagable = storagable;
        }

        public override void Execute()
        {
            if (!_storage.HasKey(_storagable.Key))
            {
                _storage.Save(_storagable);
            }
            else
            {
                _storage.Load(_storagable);
            }

            OnCompleted(true);
        }
    }
}
