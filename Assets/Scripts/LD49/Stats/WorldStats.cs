using LD49.Stats;
using UnityEngine;

namespace LD49.Enviroment
{
    public class WorldStats
    {
        public float Stable { get; private set; } = 1f;
        public float StableDelta { get; private set; } = 0.3f;
        public float MaxHp { get; private set; }
        
        public void DecreaseStable()
        {
            Stable -= StableDelta;
        }

        public void ChangeStat(StatsIds id, float newValue)
        {
            switch (id)
            {
                case StatsIds.StableDelta:
                    StableDelta = newValue;
                    break;

                case StatsIds.MaxHp:
                    MaxHp = newValue;
                    break;
                
                default:
                    Debug.LogWarning($"Stat id {id} didn't define.");
                    break;
            }
        }
    }
}