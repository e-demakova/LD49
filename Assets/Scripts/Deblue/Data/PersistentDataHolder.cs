namespace Deblue.Data
{
    public class PersistentDataHolder<T>
    {
        public T Data => _data;

        private T _data;
        private readonly string _fileName = typeof(T).Name;
        private readonly LoadService _saving;

        public PersistentDataHolder(LoadService saving)
        {
            _saving = saving;
        }

        protected void SaveData()
        {
            _saving.Save(_fileName, _data);
        }

        protected T LoadData()
        {
            _data = _saving.LoadPersistentJSON(_fileName, default(T));
            return _data;
        }
    }
}