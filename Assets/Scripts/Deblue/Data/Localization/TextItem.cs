namespace Deblue.Data.Localization
{
    public struct TextItems
    {
        public TextItem[] Items;
    }

    [System.Serializable]
    public class TextItem
    {
        public string Id;
        public string Text;
    }
}