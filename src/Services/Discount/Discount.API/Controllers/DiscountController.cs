using System.Net;
using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _repository;
        private readonly ILogger<DiscountController> _logger;

        public DiscountController(IDiscountRepository repository,
            ILogger<DiscountController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{productName}", Name = "GetDiscountAsync")]
        [ProducesResponseType(typeof(Coupon),
            (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>>
            GetDiscountAsync(string productName)
            => Ok(await _repository
                .GetDiscountAsync(productName));

        [HttpPost]
        [ProducesResponseType(typeof(Coupon),
            (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>>
            CreateCouponAsync([FromBody] Coupon coupon)
        {
            await _repository.CreateDiscountAsync(coupon);
            return CreatedAtRoute(
                "GetDiscountAsync",
                new {ProductName = coupon.ProductName},
                coupon
            );
        }


        [HttpPut]
        [ProducesResponseType(typeof(bool),
            (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>>
            UpdateCouponAsync([FromBody] Coupon coupon)
        
            => Ok(await _repository
                .UpdateDiscountAsync(coupon));


        [HttpDelete]
        [ProducesResponseType(typeof(bool),
            (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>>
            DeleteCouponAsync(string productName)

            => Ok(await _repository
                .DeleteDiscountAsync(productName));

    }
}
