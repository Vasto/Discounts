
namespace Discounts.Server.DataAccess
{
    public interface ICodesRepository
    {
        Task<bool> TryAddCode(string code);
    }
}
