namespace Events.Api.Controllers.Dtos
{
    /// <summary>
    /// Параметры зарегистрированного события
    /// </summary>
    public record EventResponseDto
    {
        /// <summary>
        /// Порядковый номер события в системе
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование события
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Подробное описание события
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Дата и время начала события в формате UTC
        /// </summary>
        public DateTime StartAt { get; set; }

        /// <summary>
        /// Дата и время окончания события в формате UTC
        /// </summary>
        public DateTime EndAt { get; set; }
    }
}
