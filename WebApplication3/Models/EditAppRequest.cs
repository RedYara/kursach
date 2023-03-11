using WebApplication3.Persistence.Models;

namespace WebApplication3.Models
{
    public class EditAppRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public IFormFile Logo { get; set; }
        public string LogoUrl { get; set; }
        public int GroupId { get; set; }
        public int GroupTypeId { get; set; }
        public IFormFileCollection Screenshots { get; set; }
        public List<Screenshots> ScreenshotsList { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public IFormFile Apk { get; set; }
    }
}