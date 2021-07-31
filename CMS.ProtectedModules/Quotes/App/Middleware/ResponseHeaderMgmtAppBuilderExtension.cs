using Microsoft.AspNetCore.Builder;
using CMS.Quotes.App.Middleware;

public static class ResponseHeaderMgmtAppBuilderExtension
{
    public static IApplicationBuilder UseSecurityHeadersMiddleware(this IApplicationBuilder app, CustomResponseHeadersBuilder builder)
    {
        CustomResponseHeadersPolicy policy = builder.Build();
        return app.UseMiddleware<CustomResponseHeaderMiddleware>(policy);
    }
}