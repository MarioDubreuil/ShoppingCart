using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.ShoppingCart
{
    public class ShoppingCart
    {
        private readonly HashSet<ShoppingCartItem> items = new();

        public int UserId { get; }
        public IEnumerable<ShoppingCartItem> Items => this.items;

        public ShoppingCart(int userId) => this.UserId = userId;

        public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems)
        {
            foreach (var item in shoppingCartItems)
            {
                this.items.Add(item);
            }
        }
        public void AddItems(int[] productCatalogueIds)
        {
            if (productCatalogueIds != null)
            {
                foreach (var productCatalogueId in productCatalogueIds)
                {
                    var item = new ShoppingCartItem(productCatalogueId, "", "", new Money("", 0));
                    this.items.Add(item);
                }
            }
        }

        public void RemoveItems(int[] productCatalogueIds) =>
            this.items.RemoveWhere(i => productCatalogueIds.Contains(i.ProductCatalogueId));
    }
}