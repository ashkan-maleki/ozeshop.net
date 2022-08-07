using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService
            .DiscountProtoServiceClient _discountService;

        public DiscountGrpcService(DiscountProtoService
            .DiscountProtoServiceClient discountService)
        {
            _discountService = discountService;
        }

        public async Task<CouponModel> GetDiscountAsync(
            string productName)
            => await _discountService.GetDiscountAsync(new
                GetDiscountRequest
                {
                    ProductName = productName
                });
    }
}
