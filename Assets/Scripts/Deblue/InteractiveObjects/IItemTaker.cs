namespace Deblue.InteractiveObjects
{
    public interface IItemTaker
    {
        bool IsCanTakeObject { get; }

        bool IsContainObject(string objId);
        HandpickedItem ReturnObject();
    }
}