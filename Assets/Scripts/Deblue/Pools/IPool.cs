using System;
using UnityEngine;

namespace Deblue.Pools
{
    public interface IPool<T> where T : IPoolItem
    {
        T GetItem();
        void ReturnToPool(T spawnObject);
        void ReturnAll();
    }
    
    public interface IPrefabPool<T> : IPool<T>, IDisposable where T : MonoBehaviour, IPoolItem 
    {
    }
}