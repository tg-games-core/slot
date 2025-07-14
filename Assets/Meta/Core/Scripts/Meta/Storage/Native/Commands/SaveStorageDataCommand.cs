namespace Core
{
    public class SaveStorageDataCommand : AbstractCommand
    {
        private Storage _storage;
        private IStoragable _storagable;
        
        public SaveStorageDataCommand(Storage storage, IStoragable storagable)
        {
            _storage = storage;
            _storagable = storagable;
        }

        public override void Execute()
        {
            _storage.Save(_storagable);
            OnCompleted(true);
        }
    }
}
