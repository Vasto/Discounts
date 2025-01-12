using Discounts.Server.Model;
using Discounts.Server.Services;
using Discounts.UnitTests.Mocks;
using Microsoft.Extensions.Logging;
using Moq;

namespace Discounts.UnitTests
{
    public class DiscountsServiceTests
    {
        private readonly ILogger<DiscountsService> _loggerMock = Mock.Of<ILogger<DiscountsService>>();
        private readonly ICodeGenerator codeGenerator = new RandomCodeGenerator();

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        public async Task GenerateCodes_GivenNumberOfCodes_ShouldGenerateCorrectCodesCount(int numberOfCodes)
        {
            //Arrange
            var codesRepositoryMock = new InMemoryCodeRepository();
            var discountsService = new DiscountsService(codeGenerator, codesRepositoryMock, _loggerMock);

            //Act
            var codes = await discountsService.GenerateCodes(numberOfCodes, 8, CancellationToken.None);

            //Assert
            Assert.Equal(numberOfCodes, codes.Count());
        }

        [Theory]
        [InlineData(10)]
        [InlineData(1000)]
        [InlineData(2000)]
        public async Task GenerateCodes_GivenNumberOfCodes_ShouldGenerateUniqueCodes(int numberOfCodes)
        {
            //Arrange
            var codesRepositoryMock = new InMemoryCodeRepository();
            var discountsService = new DiscountsService(codeGenerator, codesRepositoryMock, _loggerMock);

            //Act
            var codes = await discountsService.GenerateCodes(numberOfCodes, 8, CancellationToken.None);

            //Assert
            Assert.Equal(numberOfCodes, codes.Distinct().Count());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2001)]
        public async Task GenerateCodes_GivenOutOfRangeNumberOfCodes_ShouldReturnNull(int numberOfCodes)
        {
            //Arrange
            var codesRepositoryMock = new InMemoryCodeRepository();
            var discountsService = new DiscountsService(codeGenerator, codesRepositoryMock, _loggerMock);

            //Act
            var codes = await discountsService.GenerateCodes(numberOfCodes, 8, CancellationToken.None);

            //Assert
            Assert.Null(codes);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(6)]
        [InlineData(9)]
        public async Task GenerateCodes_GivenOutOfRangeLength_ShouldReturnNull(int length)
        {
            //Arrange
            var codesRepositoryMock = new InMemoryCodeRepository();
            var discountsService = new DiscountsService(codeGenerator, codesRepositoryMock, _loggerMock);

            //Act
            var codes = await discountsService.GenerateCodes(10, length, CancellationToken.None);

            //Assert
            Assert.Null(codes);
        }

        [Fact]
        public async Task UseCode_GivenEmptyCode_ShouldReturnCodeInvalidStatus()
        {
            //Arrange
            var codesRepositoryMock = new InMemoryCodeRepository();
            var discountsService = new DiscountsService(codeGenerator, codesRepositoryMock, _loggerMock);
            var code = "";

            //Act
            var result = await discountsService.UseCode(code);

            //Assert
            Assert.Equal(CodeUsageStatus.CodeInvalid, result);
        }

        [Fact]
        public async Task UseCode_GivenCodeThatDoesNotExist_ShouldReturnCodeInvalidStatus()
        {
            //Arrange
            var codesRepositoryMock = new InMemoryCodeRepository();
            var discountsService = new DiscountsService(codeGenerator, codesRepositoryMock, _loggerMock);
            var code = "code";

            //Act
            var result = await discountsService.UseCode(code);

            //Assert
            Assert.Equal(CodeUsageStatus.CodeInvalid, result);
        }

        [Fact]
        public async Task UseCode_GivenCodeThatIsAlreadyUsed_ShouldReturnCodeAlreadyUsedStatus()
        {
            //Arrange
            var codesRepositoryMock = new InMemoryCodeRepository();
            var discountsService = new DiscountsService(codeGenerator, codesRepositoryMock, _loggerMock);
            var code = "code";

            await codesRepositoryMock.TryAddCode(code, CodeStatus.Used);

            //Act
            var result = await discountsService.UseCode(code);

            //Assert
            Assert.Equal(CodeUsageStatus.CodeAlreadyUsed, result);
        }

        [Fact]
        public async Task UseCode_GivenCodeThatIsNotUsed_ShouldReturnSuccessStatus()
        {
            //Arrange
            var codesRepositoryMock = new InMemoryCodeRepository();
            var discountsService = new DiscountsService(codeGenerator, codesRepositoryMock, _loggerMock);
            var code = "code";

            await codesRepositoryMock.TryAddCode(code, CodeStatus.New);

            //Act
            var result = await discountsService.UseCode(code);

            //Assert
            Assert.Equal(CodeUsageStatus.Success, result);
        }
    }
}