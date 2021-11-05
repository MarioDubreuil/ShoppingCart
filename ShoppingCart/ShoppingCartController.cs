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
            var shoppingCart = shoppingCartStore.Get(userId);
            shoppingCartStore.Save(shoppingCart);
            return shoppingCart;
        }
    }
}