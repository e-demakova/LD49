using System.Collections.Generic;
using Deblue.Data;
using Deblue.ObservingSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Deblue.Pools
{
    public class Pool<T> : IPrefabPool<T> where T : MonoBehaviour, IPoolItem
    {
        private readonly LoadService _loader;
        private readonly Queue<T> _items = new Queue<T>(15);
        private readonly List<T> _itemsInScene = new List<T>(15);
        private readonly Transform _parent;
        private T _prefab;

        private readonly List<IObserver> _observers = new List<IObserver>(20);

        public Pool(T prefab, Transform parent = null) : this(parent)
        {
            _prefab = prefab;
        }

        public Pool(AssetReference assetRef, LoadService loader, int count = 0, Transform parent = null) : this(parent)
        {
            _loader = loader;
            loader.LoadAddressableMono<T>(result => _prefab = result, assetRef);
        }
        
        public Pool(LoadService loader)
        {
            _loader = loader;
        }
        
        private Pool(Transform parent)
        {
            _parent = parent;
        }

        public void LoadPrefab(AssetReference assetRef)
        {
            _loader.LoadAddressableMono<T>(result => _prefab = result, assetRef);
        }
        
        public T GetItem()
        {
            var item = _items.Count > 0 ? _items.Dequeue() : InstantiateItem();
            StartObserveItem(item);
            return item;
        }

        public void ReturnToPool(T item)
        {
            item.DeInit();
            item.LifeEnded.Unsubscribe(ReturnToPool);
            _items.Enqueue(item);
        }

        public void ReturnAll()
        {
            for (int i = 0; i < _itemsInScene.Count; i++)
            {
                ReturnToPool(_itemsInScene[i]);
            }
        }

        public void Dispose()
        {
            _observers.ClearObservers();
        }

        private T InstantiateItem()
        {
            return Object.Instantiate(_prefab, _parent);
        }

        private void StartObserveItem(T item)
        {
            _itemsInScene.Add(item);
            item.LifeEnded.Subscribe(ReturnToPool, _observers);
        }

        private void ReturnToPool(PoolItemLifeEnd context)
        {
            ReturnToPool((T) context.Item);
        }
    }
}