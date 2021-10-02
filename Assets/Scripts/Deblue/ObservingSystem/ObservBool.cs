namespace Deblue.ObservingSystem
{
    [System.Serializable]
    public class ObservBool : ObservProperty<bool>
    {
        public static implicit operator bool(ObservBool i) => i.Value;

        public static bool operator ==(ObservBool a, bool b) => (a.Value == b);
        public static bool operator !=(ObservBool a, bool b) => (a.Value != b);

        public static bool operator ==(ObservBool a, ObservBool b) => (a.Value == b.Value);
        public static bool operator !=(ObservBool a, ObservBool b) => (a.Value != b.Value);

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hashCode = Value.GetHashCode();
            return hashCode;
        }

        public ObservBool(bool value) : base(value)
        {
        }
        
        public ObservBool() : base(false)
        {
        }
    }
}
