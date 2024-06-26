namespace DB.Models
{
    public class NewAuction
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";

        public List<string> Photos { get; set; } = new();

    }
}
