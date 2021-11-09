using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.ShoppingCart
{
    [Route("/shoppingcart")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartStore shoppingCartStore;

        public ShoppingCartController(IShoppingCartStore shoppingCartStore)
        {
            this.shoppingCartStore = shoppingCartStore;
        }

        [HttpGet("{userId:int}")]
        public ShoppingCart Get(int userId) =>
            this.shoppingCartStore.Get(userId);

        [HttpPost("{userId:int}/items")]
        public ShoppingCart Post(
            int userId,
            [FromBody] int[] productIds)
        {
            var cart = shoppingCartStore.Get(userId);
            cart.AddItems(productIds);
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