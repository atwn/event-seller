using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Events.Api.Controllers.Filters
{
    /// <summary>
    /// Конвертирует все пути в документации Swagger в нижний регистр.
    /// </summary>
    public class LowerCasePathFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var newPaths = new OpenApiPaths();
            foreach (var path in swaggerDoc.Paths)
            {
                newPaths.Add(path.Key.ToLowerInvariant(), path.Value);
            }

            swaggerDoc.Paths = newPaths;
        }
    }
}
