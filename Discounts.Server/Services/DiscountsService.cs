using Discounts.Server.DataAccess;
using Discounts.Server.Model;

namespace Discounts.Server.Services
{
    public class DiscountsService : IDiscountsService
    {
        private static readonly char[] _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        private readonly ICodesRepository _codesRepository;
        private readonly ILogger<DiscountsService> _logger;

        public DiscountsService(ICodesRepository codesRepository, ILogger<DiscountsService> logger)
        {
            _codesRepository = codesRepository ?? throw new ArgumentNullException(nameof(codesRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<string>> GenerateCodes(int count, int length, CancellationToken cancellation)
        {
            bool hasGeneratedSuccessfully = true;
            var codes = new List<string>(count);
            try
            {
                for (int i = 0; i < count; i++)
                {
                    var code = GenerateSingleRandomCode(length);
                    var result = await _codesRepository.TryAddCode(code, CodeStatus.New);
                    if (!result)
                    {
                        i--;
                    }

                    codes.Add(code);
                    cancellation.ThrowIfCancellationRequested();
                }

                return codes;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, "Operation was cancelled");
                hasGeneratedSuccessfully = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating codes");
                hasGeneratedSuccessfully = false;
            }

            if (!hasGeneratedSuccessfully)
            {
                await _codesRepository.RemoveCodes(codes);
                return null;
            }

            return codes;
        }

        public async Task<CodeUsageStatus> UseCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return CodeUsageStatus.CodeInvalid;
            }

            try
            {
                var codeStatus = await _codesRepository.GetCodeStatus(code);
                if (codeStatus == null)
                {
                    return CodeUsageStatus.CodeInvalid;
                }
                else if (codeStatus == CodeStatus.Used)
                {
                    return CodeUsageStatus.CodeAlreadyUsed;
                }
                else
                {
                    // mark as used, and set expiration so it is not generated again in some time
                    var setResult = await _codesRepository.SetCodeStatus(code, CodeStatus.Used, TimeSpan.FromDays(30));
                    if (!setResult)
                    {
                        return CodeUsageStatus.CodeUsageError;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating codes");
                return CodeUsageStatus.UnexpectedError;
            }

            return CodeUsageStatus.Success;
        }

        private string GenerateSingleRandomCode(int length) => new string(Enumerable.Repeat(_characters, length)
                    .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
    }
}
