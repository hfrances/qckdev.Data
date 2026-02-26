using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qckdev.DataTest.Configuration
{
    static class ConfigurationHelper
    {


        public static Settings GetSettings(string environment = null)
        {
            var currentEnvironment = string.IsNullOrWhiteSpace(environment)
                ? (Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                    ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                    ?? "Production")
                : environment;

#if NET461_OR_GREATER || NETCOREAPP

            var builder = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{currentEnvironment}.json", true, true)
                    .AddEnvironmentVariables();

            var config = builder.Build();

            return config.Get<Settings>();
#else

            var fileName = "appsettings.json";
            var fileNameByEnv = $"appsettings.{currentEnvironment}.json";
            var settings = new Settings();

            if (System.IO.File.Exists(fileName))
            {
                using (var reader = new System.IO.StreamReader(fileName))
                {
                    Newtonsoft.Json.JsonConvert.PopulateObject(reader.ReadToEnd(), settings);
                }
            }
            if (!string.IsNullOrEmpty(currentEnvironment?.Trim()) && System.IO.File.Exists(fileNameByEnv))
            {
                using (var reader = new System.IO.StreamReader(fileNameByEnv))
                {
                    Newtonsoft.Json.JsonConvert.PopulateObject(reader.ReadToEnd(), settings);
                }
            }
            return settings;
#endif
        }


    }
}
