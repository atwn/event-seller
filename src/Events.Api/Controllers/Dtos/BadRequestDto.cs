namespace Events.Api.Controllers.Dtos
{
    /// <summary>
    /// Возвращается в случае наличия ошибок валидации входных параметров
    /// </summary>
    public class BadRequestDto
    {
        /// <summary>
        /// Описание ошибки
        /// </summary>
        public required string Message { get; set; }

        /// <summary>
        /// Код ошибки
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Список ошибок валидации для каждого проверяемого поля/значения
        /// </summary>
        public required IDictionary<string, string[]> Errors { get; set; }
    }
}
