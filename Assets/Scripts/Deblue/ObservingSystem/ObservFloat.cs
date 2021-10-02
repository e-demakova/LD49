using System;

namespace Deblue.ObservingSystem
{
    [System.Serializable]
    public class ObservFloat : ObservLimitProperty<float>
    {
        public static float Tolerance = 0.0001f;
        public static implicit operator float(ObservFloat i) => i.Value;
        public static explicit operator int(ObservFloat i) => (int)i.Value;

        public static ObservFloat operator ++(ObservFloat a)
        {
            a.Value++;
            return a;
        }

        public static ObservFloat operator --(ObservFloat a)
        {
            a.Value--;
            return a;
        }

        public static ObservFloat operator +(ObservFloat a, float b)
        {
            a.Value += b;
            return a;
        }

        public static ObservFloat operator -(ObservFloat a, float b)
        {
            a.Value -= b;
            return a;
        }

        public static ObservFloat operator *(ObservFloat a, float b)
        {
            a.Value *= b;
            return a;
        }

        public static ObservFloat operator /(ObservFloat a, float b)
        {
            a.Value /= b;
            return a;
        }

        public static bool operator >(ObservFloat a, float b) => (a.Value > b);
        public static bool operator <(ObservFloat a, float b) => (a.Value < b);

        public static bool operator >=(ObservFloat a, float b) => (a.Value >= b);
        public static bool operator <=(ObservFloat a, float b) => (a.Value <= b);

        public static bool operator ==(ObservFloat a, int b) => a.Equals(b);
        public static bool operator !=(ObservFloat a, int b) => !a.Equals(b);

        public static bool operator >(ObservFloat a, ObservFloat b) => (a.Value > b.Value);
        public static bool operator <(ObservFloat a, ObservFloat b) => (a.Value < b.Value);

        public static bool operator >=(ObservFloat a, ObservFloat b) => (a.Value >= b.Value);
        public static bool operator <=(ObservFloat a, ObservFloat b) => (a.Value <= b.Value);

        public static bool operator ==(ObservFloat a, ObservFloat b) => a.Equals(b.Value);
        public static bool operator !=(ObservFloat a, ObservFloat b) => !a.Equals(b.Value);

        public ObservFloat(float loverLimit, float upperLimit) : base(loverLimit, upperLimit)
        {
        }

        public ObservFloat(float value, float loverLimit, float upperLimit) : base(value, loverLimit, upperLimit)
        {
        }

        public ObservFloat(float value = 0) : base(value, float.MinValue, float.MaxValue)
        {
        }

        public override bool Equals(object obj)
        {
            return obj is ObservInt obsInt &&
                   Equals(obsInt.Value) &&
                   Equals(obsInt.LowerLimit) &&
                   Equals(obsInt.UpperLimit);
        }

        public bool Equals(float value)
        {
            return Math.Abs(Value - value) < Tolerance;
        }

        public override int GetHashCode()
        {
            int hashCode = -1304306776;
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + LowerLimit.GetHashCode();
            hashCode = hashCode * -1521134295 + UpperLimit.GetHashCode();
            return hashCode;
        }
    }
}
