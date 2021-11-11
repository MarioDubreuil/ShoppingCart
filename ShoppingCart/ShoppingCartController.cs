using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.ShoppingCart
{
    [Route("/shoppingcart")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartStore shoppingCartStore;
        private readonly IProductCatalogClient productCatalogClient;

        public ShoppingCartController(IShoppingCartStore shoppingCartStore, IProductCatalogClient productCatalogClient)
        {
            this.shoppingCartStore = shoppingCartStore;
            this.productCatalogClient = productCatalogClient;
        }

        [HttpGet("{userId:int}")]
        public ShoppingCart Get(int userId) =>
            this.shoppingCartStore.Get(userId);

        [HttpPost("{userId:int}/items")]
        public async Task<ShoppingCart> Post(
            int userId,
            [FromBody] int[] productIds)
        {
            var cart = shoppingCartStore.Get(userId);
            var items = await this.productCatalogClient.GetShoppingCartItems(productIds);
            cart.AddItems(items);
            shoppingCartStore.Save(cart);
            return cart;
        }

        [HttpDelete("{userId:int}/items")]
        public ShoppingCart Delete(
            int userId,
            [FromBody] int[] productIds)
        {
            var cart = this.shoppingCartStore.Get(userId);
            cart.RemoveItems(productIds);
            this.shoppingCartStore.Save(cart);
            return cart;
        }
    }
}