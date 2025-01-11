using Discounts.Server.Services;
using Grpc.Core;

namespace Discounts.Server.Controllers
{
    public class DiscountsController : Discounts.DiscountsBase
    {
        private readonly IDiscountsService _discountsService;
        private readonly ILogger<DiscountsController> _logger;

        public DiscountsController(IDiscountsService discountsService, ILogger<DiscountsController> logger)
        {
            _discountsService = discountsService ?? throw new ArgumentNullException(nameof(discountsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
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

            var generatedCodes = await _discountsService.GenerateCodes(request.Count, request.Length);

            var response = new GenerateCodeResponse() { Result = true };
            response.Codes.AddRange(generatedCodes);

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UseCodeResponse> UseCode(UseCodeRequest request, ServerCallContext context)
        {
            var useCodeResult = await _discountsService.UseCode(request.Code);
            return new UseCodeResponse { Result = useCodeResult };
        }
    }
}
