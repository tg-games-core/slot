using VContainer;

namespace Core
{
    public class StorageObject<T> : StorageObject where T : StorageData, new()
    {
        protected readonly ReactiveSubscribersContainer _reactiveContainer = new();
        
        public T ConcreteData
        {
            get => StorageData as T;
        }

        public override string Key
        {
            get => typeof(T).ToString();
        }

        [Inject]
        public StorageObject(Storage storage) : base(storage)
        {
            StorageData = new T();
        }
    }
    
    public abstract class StorageObject : IStoragable
    {
        private readonly LoadStorageDataCommand _loadCommand;
        private readonly SaveStorageDataCommand _saveCommand;

        public StorageData StorageData
        {
            get; set;
        }

        public abstract string Key
        {
            get;
        }

        [Inject]
        public StorageObject(Storage storage)
        {
            _loadCommand = new LoadStorageDataCommand(storage, this);
            _saveCommand = new SaveStorageDataCommand(storage, this);
        }

        public void Save()
        {
            ExecuteCommand(_saveCommand);
        }

        public void Load()
        {
            ExecuteCommand(_loadCommand);
        }

        private void ExecuteCommand(ICommand command)
        {
            command.Completed += OnComplete;
            command.Execute();
        }

        protected virtual void OnComplete(ICommand command, bool result)
        {
            command.Completed -= OnComplete;
        }
    }
}
