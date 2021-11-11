using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ShoppingCart.ShoppingCart;

namespace ShoppingCart
{
    public class ProductCatalogClient : IProductCatalogClient
    {
        private readonly HttpClient client;
        private static string productCatalogBaseUrl = @"https://gist.githubusercontent.com/MarioDubreuil/70f1b06769396f31cc0a1237d4ee9c7a/raw/4f501b9cb5cb036695b33ac93a5bd2998e11d57f/mock-product-catalog-response.json";
        private static string getProductPathTemplate = "?productIds=[{0}]";
        public ProductCatalogClient(HttpClient client)
        {
            client.BaseAddress = new System.Uri(productCatalogBaseUrl);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            this.client = client;
        }
        public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds)
        {
            using var response = await RequestProductFromProductCatalog(productCatalogIds);
            return await ConvertToShoppingCartItems(response);
        }
        private async Task<HttpResponseMessage> RequestProductFromProductCatalog(int[] productCatalogIds)
        {
            var productsResource= string.Format(getProductPathTemplate, string.Join(",", productCatalogIds));
            return await this.client.GetAsync(productsResource);
        }
        private static async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var products = await JsonSerializer.DeserializeAsync<List<ProductCatalogProduct>>(
                await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new();
            return products.Select(p => new ShoppingCartItem(
                p.ProductId,
                p.ProductName,
                p.ProductDescription,
                p.Price
            ));
        }
        private record ProductCatalogProduct(
            int ProductId,
            string ProductName,
            string ProductDescription,
            Money Price);
    }
}
