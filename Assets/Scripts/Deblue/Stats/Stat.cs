using System;
using System.Linq;

namespace Deblue.Stats
{
    public class Stat
    {
        public float Value;

        public static TStat[] GetFullArray<TEnum, TStat>()
            where TEnum : Enum where TStat : Stat<TEnum>, new()
        {
            var ids = Enum.GetValues(typeof(TEnum));
            var globalStats = new TStat[ids.Length];
            
            for (int i = 0; i < ids.Length; i++)
            {
                var id = (TEnum) ids.GetValue(i);
                globalStats[i] = new TStat() {Id = id};
            }

            return globalStats;
        }

        public static void InsertValues<TEnum>(Stat<TEnum>[] from, Stat<TEnum>[] to) where TEnum : Enum
        {
            if (from == null || to == null)
            {
                return;
            }

            for (int i = 0; i < to.Length; i++)
            {
                var stat = from.FirstOrDefault(x => Equals(x.Id, to[i].Id));
                if (stat != null)
                {
                    to[i].Value = stat.Value;
                }
            }
        }
    }

    public class Stat<TEnum> : Stat where TEnum : Enum
    {
        public TEnum Id;
    }
}           