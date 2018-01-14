namespace Monzo.Locations.Framework.Contracts
{
    using Monzo.Locations.Framework.Entities;

    /// <summary>
    /// Encapsulates logic to interact with the Monzo API. 
    /// </summary>
    public interface IMonzoService
    {
        /// <summary>
        /// Gets authentication details. 
        /// </summary>
        /// <returns>The authentication object.</returns>
        Authentication GetAuthentication();

        /// <summary>
        /// Gets the Accounts of a User. 
        /// </summary>
        /// <returns>The accounts.</returns>
        Accounts GetAccounts();

        /// <summary>
        /// Gets the transactions for a specific account. 
        /// </summary>
        /// <returns>The transactions.</returns>
        Transactions GetTransactions(Account account);

        /// <summary>
        /// Gets all the non-online transactions for a specific account. 
        /// </summary>
        /// <returns>The physical transactions.</returns>
        /// <param name="account">Account.</param>
        Transactions GetPhysicalTransactions(Account account);
    }
}