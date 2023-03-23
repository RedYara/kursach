using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public enum Roles
    {
        [Display(Name = "Пользователь")]
        [Description("user")]
        User = 10,
        [Display(Name = "Администратор")]
        [Description("root")]
        Admin = 20,
    }

}
