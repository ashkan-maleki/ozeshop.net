using System.Net;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _repository;
    private readonly ILogger<BasketController> _logger;
    private readonly DiscountGrpcService _discountService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;


    public BasketController(
        IBasketRepository repository,
        ILogger<BasketController> logger,
        DiscountGrpcService discountService,
        IMapper mapper, 
        IPublishEndpoint publishEndpoint)
    {
        _repository = repository;
        _logger = logger;
        _discountService = discountService;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet("{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart),
        (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>>
        GetBasket(string userName)
        => Ok(await
            _repository.GetBasket(userName) ??
              new ShoppingCart(userName));

    [HttpPost()]
    [ProducesResponseType(typeof(ShoppingCart),
        (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>>
        UpdateBasket([FromBody] ShoppingCart basket)
    {
        // TODO: communicate with Discount.Grpc
        // TODO: and calculate latest price of products
        // 1. loop thorough items in the basket (cart)
        foreach (ShoppingCartItem item in basket.Items!)
        {
            //2. consume discount gRPC
            CouponModel couponModel = await _discountService
                .GetDiscountAsync(item.ProductName!);
            if (couponModel != null)
            {
                item.Price -= couponModel.Amount;
            }

        }




        return Ok(await _repository.UpdateBasket(basket));
    }


    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(ShoppingCart),
        (int)HttpStatusCode.OK)]
    public async Task<ActionResult>
        DeleteBasket([FromBody] string userName)
    {
        await _repository.DeleteBasket(userName);
        return Ok();
    }

    [HttpPost("[action]")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        // get existing basket with total price
        // Create basketCheckoutEvent -- set TotalPrice on basketCheckout eventMessage
        // send checkout event to rabbitmq
        // remove the basket

        ShoppingCart? basket = await _repository.GetBasket(basketCheckout!.UserName!);
        if (basket == null)
        {
            return BadRequest();
        }

        BasketCheckoutEvent eventMessage = 
            _mapper.Map<BasketCheckoutEvent>(basketCheckout);
        eventMessage.TotalPrice = basket.TotalPrice;
        await _publishEndpoint.Publish(eventMessage);

        await _repository.DeleteBasket(basketCheckout!.UserName!);

        return Accepted();
    }
}
