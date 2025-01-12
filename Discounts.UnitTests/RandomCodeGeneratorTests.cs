using Discounts.Server.Services;

namespace Discounts.UnitTests
{
    public class RandomCodeGeneratorTests
    {
        private static readonly char[] _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        [Theory]
        [InlineData(4)]
        [InlineData(6)]
        [InlineData(8)]
        public void GenerateSingleCode_GivenLength_ShouldReturnCodeOfSpecifiedLength(int length)
        {
            //Arrange 
            var randomCodeGenerator = new RandomCodeGenerator();

            //Act
            var result = randomCodeGenerator.GenerateSingleCode(length);

            //Assert
            Assert.Equal(length, result.Length);
        }

        [Fact]
        public void GenerateMultipleCodes_ShouldReturnCodeWithOnlyAllowedCharacters()
        {
            //Arrange 
            var randomCodeGenerator = new RandomCodeGenerator();

            //Act
            var result = randomCodeGenerator.GenerateSingleCode(8);

            //Assert
            Assert.True(result.All(c => _characters.Contains(c)));
        }
    }
}
