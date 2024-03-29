﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System.Net;

namespace Shopping.Aggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
            _orderService = orderService;
        }

        [HttpGet("{userName}",Name ="GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            // get basket with username
            // itreate basket items and consume products with basket item productid member
            // map product related member into basketitem dto with extended column
            // consume ordering microservices in order to retrieve order list
            // return root ShoppngModel dto class which including all response

            var baskets =await _basketService.GetBasket(userName);
            foreach (var item in baskets.Items)
            {
                var product = await _catalogService.GetCatalog(item.ProductId);
                // set additional product fields onto basket item
                item.ProductName = product.Name;
                item.Category= product.Category;
                item.Summary= product.Summary;
                item.Description= product.Description;
                item.ImageFile= product.ImageFile;
            }

            var orders=await _orderService.GetOrdersByUserName(userName);
            var shoppingModel=new ShoppingModel()
            {
                UserName= userName,
                BasketWithProducts=baskets,
                Orders=orders
            };
            return Ok(shoppingModel);
        }
    }
}
