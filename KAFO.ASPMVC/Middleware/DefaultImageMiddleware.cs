namespace KAFO.ASPMVC.Middleware
{
    public class DefaultImageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public DefaultImageMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            // Only handle 404 for image requests
            if (context.Response.StatusCode == 404 &&
                context.Request.Path.Value.StartsWith("/images/Upload/") &&
                Path.HasExtension(context.Request.Path.Value))
            {
                var defaultImagePath = Path.Combine(_env.WebRootPath, "images", "product.png");
                if (File.Exists(defaultImagePath))
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 200;
                    await context.Response.SendFileAsync(defaultImagePath);
                }
            }
        }
    }
}
