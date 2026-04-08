using System.ComponentModel.DataAnnotations;

namespace Events.Api.Controllers.Dtos
{
    [Validators.EndAfterStart]
    public class EventCreateDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле 'Title' обязательно для заполнения.")]
        public required string Title { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Поле 'StartAt' обязательно для заполнения.")]
        [Range(typeof(DateTime), "1900-01-01", "2100-12-31", ErrorMessage = "Поле 'StartAt' должно быть корректной датой.")]
        public DateTime? StartAt { get; set; }  // делаем nullable, чтобы EndAfterStartAttribute мог проверять на null, а не сверять с default(DateTime)

        [Required(ErrorMessage = "Поле 'EndAt' обязательно для заполнения.")]
        [Range(typeof(DateTime), "1900-01-01", "2100-12-31", ErrorMessage = "Поле 'EndAt' должно быть корректной датой.")]
        public DateTime? EndAt { get; set; }    // делаем nullable, чтобы EndAfterStartAttribute мог проверять на null, а не сверять с default(DateTime)
    }
}
