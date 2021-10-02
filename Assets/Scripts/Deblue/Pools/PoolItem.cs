using Deblue.ObservingSystem;

namespace Deblue.Pools
{
    public interface IPoolItem
    {
        IReadOnlyHandler<PoolItemLifeEnd> LifeEnded { get; }
        void DeInit();
    }
}