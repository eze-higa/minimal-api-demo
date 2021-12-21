namespace minimal_api_demo.Endpoints
{
    public static class HomeEndpoint
    {
        public static WebApplication AddHomeEndpoint(this WebApplication webApplication)
        {
            webApplication.MapGet("/api/", () => Results.Ok());
            return webApplication;
        }
    }
}
