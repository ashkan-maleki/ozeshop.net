﻿using System;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class CartModel : PageModel
    {
        private readonly IBasketService _basketService;

        public CartModel(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public BasketModel Cart { get; set; } = new ();        

        public async Task<IActionResult> OnGetAsync()
        {
            string userName = "swn";
            Cart = await _basketService.GetBasket(userName);            

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(string productId)
        {
            string userName = "swn";
            BasketModel basket = await _basketService.GetBasket(userName);

            BasketItemModel item = basket.Items
                .Single(x => x.ProductId == productId);

            basket.Items.Remove(item);

            var basketUpdated = 
                await _basketService.UpdateBasket(basket);

            return RedirectToPage();
        }
    }
}