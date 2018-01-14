namespace Monzo.Locations.Framework.Services
{
    using System;
    using Monzo.Locations.Framework.Contracts;

    /// <inheritdoc />
    public class ConfigurationService : IConfigurationService
    {
        /// <inheritdoc />
        public string GetEnviroment(string name)
        {           
            return Environment.GetEnvironmentVariable(name);
        }
    }
}