namespace Monzo.Locations.Framework.Contracts
{
    using System;

    /// <summary>
    /// Encapsualtes logic for retrieving configuaration values. 
    /// </summary>
    public interface IConfigurationService : IDisposable
    {
        /// <summary>
        /// Gets the value of the passed Enviroment variable.
        /// </summary>
        /// <returns>The enviroment variable value</returns>
        /// <param name="name">the environment variable name.</param>
        string GetEnviroment(string name); 
    }
}