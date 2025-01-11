
namespace Discounts.Server.DataAccess
{
    public interface ICodesRepository
    {
        Task<bool> TryAddCode(string code, string status);
        Task RemoveCodes(IEnumerable<string> codes);
        Task<string> GetCodeStatus(string code);
        Task<bool> SetCodeStatus(string code, string status, TimeSpan? expiration);
    }
}
