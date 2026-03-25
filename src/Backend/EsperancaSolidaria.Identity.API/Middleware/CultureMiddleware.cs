using System.Globalization;

namespace EsperancaSolidaria.Identity.API.Middleware
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Lista com todas as cultures suportadas pelo .NET
            var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures);

            // Recuperar a cultura da request
            var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            var cultureInfo = new CultureInfo("en");

            if(!string.IsNullOrWhiteSpace(requestedCulture) && supportedLanguages.Any(c => c.Name.Equals(requestedCulture)))
            {
                cultureInfo = new CultureInfo(requestedCulture);
            }

            // Trocar a cultura do sistema
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            await _next(context);
        }
    }
}