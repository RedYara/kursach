namespace WebApplication3.Persistence.Models
{
    public class GroupType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupId { get; set; }
        public ICollection<App> Apps { get; set; }
        public Group Group { get; set; }
    }
}


