using Discounts.Server.DataAccess;

namespace Discounts.UnitTests.Mocks
{
    public class InMemoryCodeRepository : ICodesRepository
    {
        private readonly Dictionary<string, string> _codes = new Dictionary<string, string>();

        public Task<string> GetCodeStatus(string code)
        {
            if (_codes.TryGetValue(code, out var status))
            {
                return Task.FromResult(status);
            }
            else
            {
                return Task.FromResult<string>(null);
            }
        }

        public Task RemoveCodes(IEnumerable<string> codes)
        {
            foreach (var code in codes)
            {
                _codes.Remove(code);
            }
            return Task.CompletedTask;
        }

        public Task<bool> SetCodeStatus(string code, string status, TimeSpan? expiration)
        {
            _codes[code] = status;
            return Task.FromResult(true);
        }

        public Task<bool> TryAddCode(string code, string status)
        {
            if (!_codes.ContainsKey(code))
            {
                _codes[code] = status;
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }
    }
}
