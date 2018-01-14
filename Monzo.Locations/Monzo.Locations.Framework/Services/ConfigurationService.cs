using System;
using Monzo.Locations.Framework.Contracts;

namespace Monzo.Locations.Framework.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public ConfigurationService()
        {
        }

        public string GetEnviroment(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
    }
}
