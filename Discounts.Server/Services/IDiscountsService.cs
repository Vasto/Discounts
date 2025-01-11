
using Discounts.Server.Model;

namespace Discounts.Server.Services
{
    public interface IDiscountsService
    {
        Task<IEnumerable<string>> GenerateCodes(int count, int length, CancellationToken cancellation);
        Task<CodeUsageStatus> UseCode(string code);
    }
}