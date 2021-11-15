using System.Collections.Generic;
using System.Linq;
using ShoppingCart.EventFeed;

namespace ShoppingCart.ShoppingCart
{
    public class ShoppingCart
    {
        private readonly HashSet<ShoppingCartItem> items = new();

        public int UserId { get; }
        public IEnumerable<ShoppingCartItem> Items => this.items;

        public ShoppingCart(int userId) => this.UserId = userId;

        public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IEventStore eventStore)
        {
            foreach (var item in shoppingCartItems)
            {
                if(this.items.Add(item))
                {
                    eventStore.Raise("ShoppingCartItemAdded", new { UserId, item });
                }
            }
        }

        public void RemoveItems(int[] productCatalogIds, IEventStore eventStore)
        {
            foreach (var productCatalogId in productCatalogIds)
            {
                int numberOfElementsRemoved = this.items.RemoveWhere(i => i.ProductCatalogId == productCatalogId);
                if (numberOfElementsRemoved > 0)
                {
                    eventStore.Raise("ShoppingCartItemRemoved", new {UserId, productCatalogId});
                }
            }
        }
    }
}