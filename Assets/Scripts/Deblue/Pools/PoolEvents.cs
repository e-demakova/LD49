namespace Deblue.Pools
{
    public readonly struct PoolItemLifeEnd
    {
        public readonly IPoolItem Item;

        public PoolItemLifeEnd(IPoolItem item)
        {
            Item = item;
        }
    }
}