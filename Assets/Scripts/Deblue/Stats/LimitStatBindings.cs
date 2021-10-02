using System;

namespace Deblue.Stats
{
    public struct LimitStatBindings<TMaxValue, TStat>
        where TMaxValue : Enum where TStat : Enum
    {
        public ModifiableStatsStorage<TMaxValue> ModifiableStats;
        public LimitedStatsStorage<TStat> LimitedStats;
        public TMaxValue MaxStatValueId;
        public TStat StatId;

        public static void SubscribeStatLimit(LimitStatBindings<TMaxValue, TStat> bindings)
        {
            var maxStatValue = bindings.ModifiableStats.GetStatProperty(bindings.MaxStatValueId);
            bindings.LimitedStats.SubscribeUpperLimit(maxStatValue, bindings.StatId);
        }
    }
}