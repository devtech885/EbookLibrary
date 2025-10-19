using Microsoft.AspNetCore.Builder;

namespace Web.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerWithUi(this WebApplication app)
        {
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "OpenApi V1");
            });

            return app;
        }
    }
}
