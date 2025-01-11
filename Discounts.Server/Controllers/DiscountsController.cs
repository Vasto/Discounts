using Discounts.Server.Services;
using Grpc.Core;

namespace Discounts.Server.Controllers
{
    public class DiscountsController : Discounts.DiscountsBase
    {
        private readonly IDiscountsService _discountsService;

        public DiscountsController(IDiscountsService discountsService)
        {
            _discountsService = discountsService ?? throw new ArgumentNullException(nameof(discountsService));
        }
    
        /// <summary>
        /// Generate given number of discount codes and return them
        /// </summary>
        public override async Task<GenerateCodeResponse> GenerateCode(GenerateCodeRequest request, ServerCallContext context)
        {
            if (request.Count <= 1 || request.Count > 2000)
            {
                return new GenerateCodeResponse { Result = false };
            }

            if (request.Length < 7 || request.Length > 8)
            {
                return new GenerateCodeResponse { Result = false };
            }

            var generatedCodes = await _discountsService.GenerateCodes(request.Count, request.Length, context.CancellationToken);
            if (generatedCodes == null)
            {
                return new GenerateCodeResponse { Result = false };
            }

            var response = new GenerateCodeResponse() { Result = true };
            response.Codes.AddRange(generatedCodes);

            return response;
        }

        /// <summary>
        /// Sets a discount code as used
        /// </summary>
        public override async Task<UseCodeResponse> UseCode(UseCodeRequest request, ServerCallContext context)
        {
            var useCodeResult = await _discountsService.UseCode(request.Code);
            return new UseCodeResponse { Result = (int)useCodeResult };
        }
    }
}
