namespace WebApplication3.Persistence.Models
{
    public class App
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public int GroupId { get; set; }
        public int GroupTypeId { get; set; }
        public Group Group { get; set; }
        public GroupType GroupType { get; set; }
        public List<Screenshots> Screenshots { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string DownloadLink { get; set; }
    }
}
