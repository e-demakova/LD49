namespace Deblue.ObservingSystem
{
    [System.Serializable]
    public class ObservInt : ObservLimitProperty<int>
    {
        public static implicit operator int(ObservInt i) => i.Value;
        public static explicit operator float(ObservInt i) => i.Value;

        public static ObservInt operator ++(ObservInt a)
        {
            a.Value++;
            return a;
        }
        
        public static ObservInt operator --(ObservInt a)
        {
            a.Value--;
            return a;
        }
        
        public static ObservInt operator +(ObservInt a, int b)
        {
            a.Value += b;
            return a;
        }
        
        public static ObservInt operator -(ObservInt a, int b)
        {
            a.Value -= b;
            return a;
        }
        
        public static ObservInt operator *(ObservInt a, int b)
        {
            a.Value *= b;
            return a;
        }
        
        public static ObservInt operator /(ObservInt a, int b)
        {
            a.Value /= b;
            return a;
        }

        public static bool operator >(ObservInt a, int b) => (a.Value > b);
        public static bool operator <(ObservInt a, int b) => (a.Value < b);
        
        public static bool operator >=(ObservInt a, int b) => (a.Value >= b);
        public static bool operator <=(ObservInt a, int b) => (a.Value <= b);

        public static bool operator ==(ObservInt a, int b) => (a.Value == b);
        public static bool operator !=(ObservInt a, int b) => (a.Value != b);
        
        public static bool operator >(ObservInt a, ObservInt b) => (a.Value > b.Value);
        public static bool operator <(ObservInt a, ObservInt b) => (a.Value < b.Value);
        
        public static bool operator >=(ObservInt a, ObservInt b) => (a.Value >= b.Value);
        public static bool operator <=(ObservInt a, ObservInt b) => (a.Value <= b.Value);

        public static bool operator ==(ObservInt a, ObservInt b) => (a.Value == b.Value);
        public static bool operator !=(ObservInt a, ObservInt b) => (a.Value != b.Value);

        public ObservInt() : base(0, int.MinValue, int.MaxValue)
        {
        }

        public ObservInt(int value, int loverLimit, int upperLimit) : base(value, loverLimit, upperLimit)
        {
        }
        
        public ObservInt(int value) : base(value, int.MinValue, int.MaxValue)
        {
        }

        public override bool Equals(object obj)
        {
            return obj is ObservInt obsInt &&
                   Value == obsInt.Value &&
                   LowerLimit == obsInt.LowerLimit &&
                   UpperLimit == obsInt.UpperLimit;
        }

        public override int GetHashCode()
        {
            int hashCode = -169172844;
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + LowerLimit.GetHashCode();
            hashCode = hashCode * -1521134295 + UpperLimit.GetHashCode();
            return hashCode;
        }
    }
}
