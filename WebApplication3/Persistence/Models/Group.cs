using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Persistence.Models
{
    public class Group
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Укажите название группы")]
        public string Name { get; set; }
        public ICollection<App> Apps { get; set; }
        public ICollection<GroupType> GroupTypes { get; set; }
    }
}


