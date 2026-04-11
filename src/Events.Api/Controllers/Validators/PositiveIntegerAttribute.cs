using System.ComponentModel.DataAnnotations;

namespace Events.Api.Controllers.Validators
{
    public class PositiveIntegerAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            return value is int integer && integer > 0
                ? ValidationResult.Success
                : new ValidationResult($"""
                    Параметр '{context.MemberName}' должен быть положительным целым числом и не превышать {int.MaxValue}.
                    """);
        }
    }
}
