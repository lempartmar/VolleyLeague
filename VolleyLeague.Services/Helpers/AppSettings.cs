using Microsoft.Extensions.Configuration;

namespace VolleyLeague.Services.Helpers
{
    public static class AppSettings
    {
        public static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
