using System.ComponentModel.DataAnnotations;

namespace Events.Api.Controllers.Validators
{
    public class EndAfterStartAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            if (value is Dtos.EventCreateDto eventDto
                && eventDto.EndAt.HasValue && eventDto.StartAt.HasValue // проверяем, что оба поля имеют значение,
                                                                        // потому что этот валидатор может быть применен раньше атрибута Required
                && eventDto.EndAt <= eventDto.StartAt)
            {
                return new ValidationResult("Поле 'EndAt' должно быть позже поля 'StartAt'.");
            }

            return ValidationResult.Success;
        }
    }
}
