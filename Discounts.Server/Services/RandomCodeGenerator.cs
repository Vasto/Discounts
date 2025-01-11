namespace Discounts.Server.Services
{
    public class RandomCodeGenerator : ICodeGenerator
    {
        private static readonly char[] _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        public string GenerateSingleCode(int length) => new string(Enumerable.Repeat(_characters, length)
                    .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
    }
}
