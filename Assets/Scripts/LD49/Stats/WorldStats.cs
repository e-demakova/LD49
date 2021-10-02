namespace LD49.Enviroment
{
    public class WorldStats
    {
        private float _stable;
     
        public float Stable => _stable;
        public float StableDelta { get; set; } = 0.3f;

        public WorldStats(float stable)
        {
            _stable = stable;
        }

        public void DecreaseStable()
        {
            _stable -= StableDelta;
        }
    }
}