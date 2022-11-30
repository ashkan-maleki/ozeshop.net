using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;

namespace Shopping.Aggregator.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ShoppingController : ControllerBase
{
    private readonly ICatalogService _catalogService;
    private readonly IBasketService _basketService;
    private readonly IOrderService _orderService;

    public ShoppingController(ICatalogService catalogService, 
        IBasketService basketService, 
        IOrderService orderService)
    {
        _catalogService = catalogService;
        _basketService = basketService;
        _orderService = orderService;
    }

    [HttpGet("{userName}", Name = "GetShopping")]
    [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingModel>> 
        GetShopping(string userName)
    {
        // get basket with username
        // iterate basket items and consume products with basket item productId member
        // map product related members into basketitem dto with extended columns
        // consume ordering microservices in order to retrieve order list
        // return root ShoppngModel dto class which including all responses

        var basket = await _basketService.GetBasket(userName);

        if (basket?.Items != null)
            foreach (BasketItemExtendedModel item in basket.Items)
            {
                CatalogModel product = await _catalogService
                    .GetCatalog(item!.ProductId!);

                // set additional product fields onto basket item
                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }

        IEnumerable<OrderResponseModel> orders = 
            await _orderService.GetOrderByUserName(userName);

        ShoppingModel shoppingModel = new()
        {
            UserName = userName,
            BasketWithProducts = basket,
            Orders = orders
        };

        return Ok(shoppingModel);
    }
}