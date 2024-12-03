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


        public static Settings GetSettings(string environment = "Development")
        {

#if NET461_OR_GREATER || NETCOREAPP

            var builder = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{environment}.json", true, true)
                    .AddEnvironmentVariables();

            var config = builder.Build();

            return config.Get<Settings>();
#else

            var fileName = "appsettings.json";
            var fileNameByEnv = $"appsettings.{environment}.json";
            var settings = new Settings();

            if (System.IO.File.Exists(fileName))
            {
                using (var reader = new System.IO.StreamReader(fileName))
                {
                    Newtonsoft.Json.JsonConvert.PopulateObject(reader.ReadToEnd(), settings);
                }
            }
            if (!string.IsNullOrEmpty(environment?.Trim()) && System.IO.File.Exists(fileNameByEnv))
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
