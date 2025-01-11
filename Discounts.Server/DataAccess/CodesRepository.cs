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
        public async Task<bool> TryAddCode(string code)
        {
            var db = _connection.GetDatabase();
            var result = await db.StringSetAsync(code, 0, when: When.NotExists);
            return result;
        }

        /// <summary>
        /// Checks if code exists and is not used
        /// </summary>
        public async Task<string> GetCodeStatus(string code)
        {
            var db = _connection.GetDatabase();
            var result = await db.StringGetAsync(code);
            return result;
        }
    }
}
