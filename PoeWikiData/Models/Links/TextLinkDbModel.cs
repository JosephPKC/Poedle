namespace PoeWikiData.Models.Links
{
    internal class TextLinkDbModel : BaseDbModel
    {
        public string Text { get; set; } = string.Empty;
        public ushort Order { get; set; } = 0;
    }
}
