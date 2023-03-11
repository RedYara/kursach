
using System.ComponentModel.DataAnnotations;
using WebApplication3.Persistence.Models;

namespace WebApplication3.Models
{
    public class CreateAppRequest
    {
        public int Id { get; set; }
        [RegularExpression(@"^[\sА-Яа-яЁёa-zA-Z$&+,:;=?@#|""«»'<>.-^*()%!_-]+$", ErrorMessage = "Введены недопустимые символы")]
        [Required(ErrorMessage = "Укажите название")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Укажите версию")]
        public string Version { get; set; }
        [RegularExpression(@"^[\sА-Яа-яЁёa-zA-Z$&+,:;=?@#|""«»'<>.-^*()%!_-]+$", ErrorMessage = "Введены недопустимые символы")]
        [Required(ErrorMessage = "Укажите описание")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Укажите группу")]
        public int GroupId { get; set; }
        [Required(ErrorMessage = "Укажите тип приложений")]
        public int GroupTypeId { get; set; }
        [Required(ErrorMessage = "Выберите логотип")]
        public IFormFile Logo { get; set; }
        [Required(ErrorMessage = "Выберите скриншоты")]
        public IFormFileCollection Screenshots { get; set; }
        [Required(ErrorMessage = "Укажите дату")]
        public DateTimeOffset? CreatedDate { get; set; }
        [Required(ErrorMessage = "Выберите файл")]
        public IFormFile Apk { get; set; }
    }
}