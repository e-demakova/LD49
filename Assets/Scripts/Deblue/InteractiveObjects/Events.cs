namespace Deblue.InteractiveObjects
{
    public readonly struct InteractObjectUpdated
    {
        public readonly InteractItem Obj;

        public InteractObjectUpdated(InteractItem obj)
        {
            Obj = obj;
        }
    }
}