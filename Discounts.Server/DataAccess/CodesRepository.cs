using StackExchange.Redis;

namespace Discounts.Server.DataAccess
{
    public class CodesRepository : ICodesRepository
    {
        IConnectionMultiplexer _connection;

        public CodesRepository(IConnectionMultiplexer connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Adds code if it doesn't exist
        /// </summary>
        public async Task<bool> TryAddCode(string code, string status)
        {
            var db = _connection.GetDatabase();
            var result = await db.StringSetAsync(code, status, when: When.NotExists);
            return result;
        }

        /// <summary>
        /// Removes codes provided in the collection from the database
        /// </summary>
        public async Task RemoveCodes(IEnumerable<string> codes)
        {
            var db = _connection.GetDatabase();
            var transaction = db.CreateTransaction();
            foreach (var code in codes)
            {
                _ = transaction.KeyDeleteAsync(code);
            }

            await transaction.ExecuteAsync();
        }

        /// <summary>
        /// Gets code status or null if it doesn't exist
        /// </summary>
        public async Task<string> GetCodeStatus(string code)
        {
            var db = _connection.GetDatabase();
            var result = await db.StringGetAsync(code);
            return result;
        }

        /// <summary>
        /// Sets code status to a provided value, and optionally sets expiration time.
        /// </summary>
        public async Task<bool> SetCodeStatus(string code, string status, TimeSpan? expiration)
        {
            var db = _connection.GetDatabase();
            return await db.StringSetAsync(code, status, expiry: expiration);
        }
    }
}
