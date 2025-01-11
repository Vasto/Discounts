
namespace Discounts.Server.Services
{
    public interface IDiscountsService
    {
        Task<IEnumerable<string>> GenerateCodes(int count, int length);
        Task<int> UseCode(string code);
    }
}