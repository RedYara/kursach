namespace WebApplication3.Persistence.Models
{
    public class Screenshots
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public App App { get; set; }
        public string Base64 { get; set; }
    }
}


