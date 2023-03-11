using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

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
