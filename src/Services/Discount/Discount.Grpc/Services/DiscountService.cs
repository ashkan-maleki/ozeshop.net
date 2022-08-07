using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DiscountService> _logger;


        public DiscountService(IDiscountRepository repository,
            ILogger<DiscountService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<CouponModel> 
            GetDiscount(GetDiscountRequest request, 
                ServerCallContext context)
        {
            var coupon = await _repository
                .GetDiscountAsync(request.ProductName);

            if (coupon == null)
            {
                throw new RpcException(new Status(
                    StatusCode.NotFound,
                    $"Discount with ProductName=" +
                    $"{request.ProductName} is not found."
                ));
            }

            _logger.LogInformation($"Discount is retrieved for ProductName : {coupon.ProductName}, Amount : {coupon.Amount}");

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            var success =  await _repository
                .CreateDiscountAsync(coupon);
            if (success)
            {
                _logger.LogInformation($"Discount is successfully created. ProductName : {coupon.ProductName}");
            }
            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> 
            UpdateDiscount(UpdateDiscountRequest request, 
                ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            var success = await _repository
                .UpdateDiscountAsync(coupon);

            if (success)
            {
                _logger.LogInformation($"Discount is successfully updated. ProductName : {coupon.ProductName}");
            }
            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }


        public override async Task<DeleteDiscountReply> 
            DeleteDiscount(DeleteDiscountRequest request,
                ServerCallContext context)

            => new DeleteDiscountReply
            {
                Success = await _repository.DeleteDiscountAsync(request.ProductName)
            };
        
    }
}
