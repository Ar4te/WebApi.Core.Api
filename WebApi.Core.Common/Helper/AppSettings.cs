using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace WebApi.Core.Common.Helper
{
    public class AppSettings
    {
        public static IConfiguration _configuration { get; set; }

        public static string contentPath { get; set; }

        public AppSettings()
        {
            string path = "appsettings.json";
            // using Microsoft.Extensions.Configuration;
            _configuration = new ConfigurationBuilder()
                // using Microsoft.Extensions.Configuration.Json;
                .SetBasePath(contentPath)
                .Add(new JsonConfigurationSource
                {
                    Path = path,
                    Optional = false,
                    ReloadOnChange = true,
                })
                .Build();
        }

        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static string app(params string[] sections)
        {
            try
            {
                if (sections.Any())
                    return _configuration[string.Join(":", sections)];
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        public static List<T> app<T>(params string[] sections)
        {
            List<T> list = new();
            // Microsoft.Extensions.Configuration.Binder
            _configuration.Bind(string.Join(":", sections), list);
            return list;
        }
    }
}
