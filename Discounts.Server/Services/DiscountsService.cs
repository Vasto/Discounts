using Discounts.Server.DataAccess;
using System;

namespace Discounts.Server.Services
{
    public class DiscountsService : IDiscountsService
    {
        private static readonly char[] _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        private readonly ICodesRepository _codesRepository;

        public DiscountsService(ICodesRepository codesRepository)
        {
            _codesRepository = codesRepository ?? throw new ArgumentNullException(nameof(codesRepository));
        }

        public async Task<IEnumerable<string>> GenerateCodes(int count, int length)
        {
            var codes = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                var code = GenerateSingleRandomCode(length);
                var result = await _codesRepository.TryAddCode(code);
                if (!result)
                {
                    i--;
                }

                codes.Add(code);
            }

            return codes;
        }

        public async Task<int> UseCode(string code)
        {
            return await _codesRepository.UpdateCode(code,);
        }

        private string GenerateSingleRandomCode(int length) => new string(Enumerable.Repeat(_characters, length)
                    .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
    }
}
