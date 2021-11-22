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
            var items = this.items.Where(i => productCatalogIds.Contains(i.ProductCatalogId)).ToList();
            foreach (var item in items)
            {
                if(this.items.Remove(item))
                {
                    eventStore.Raise("ShoppingCartItemRemoved", new { UserId, item });
                }
            }
        }
    }
}