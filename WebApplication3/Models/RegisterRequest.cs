using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Укажите логин")]
        [RegularExpression(@"^[\sА-Яа-яЁёa-zA-Z$&+,:;=?@#|""«»'<>.-^*()%!_-]+$", ErrorMessage = "Введены недопустимые символы")]
        [Remote(action: "CheckUsername", controller: "Account", ErrorMessage = "Логин уже используется")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Укажите почту")]
        [RegularExpression(@"^[a-zA-Z0-9]+(?:\.[a-zA-Z0-9]+)*@[a-zA-Z0-9]+(?:\.[a-zA-Z0-9]+)*$", ErrorMessage = "Почта указана неверно")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Почта уже используется")]
        public string Email{ get; set; }

        [UIHint("password")]
        [Required(ErrorMessage = "Укажите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}