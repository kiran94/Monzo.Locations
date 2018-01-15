namespace Monzo.Locations.Framework.Contracts
{
    using System;
    using Monzo.Locations.Framework.Entities;

    /// <summary>
    /// Encapsulates logic to interact with the Monzo API. 
    /// </summary>
    public interface IMonzoService : IDisposable
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
        /// Gets all the offline transactions for a specific account. 
        /// </summary>
        /// <returns>The physical transactions.</returns>
        /// <param name="account">Account.</param>
        Transactions GetPhysicalTransactions(Account account);

        /// <summary>
        /// Gets all the offline transactions for a specifi account and specific date time period. 
        /// </summary>
        /// <returns>The physical transactions by date.</returns>
        /// <param name="account">Account.</param>
        /// <param name="start">Start.</param>
        /// <param name="end">End.</param>
        Transactions GetPhysicalTransactionsByDate(Account account, DateTime start, DateTime end); 
    }
}