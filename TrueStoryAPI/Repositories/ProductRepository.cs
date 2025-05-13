using Microsoft.AspNetCore.WebUtilities;
using TrueStoryAPI.DTOs;
using TrueStoryAPI.Interfaces;
using TrueStoryAPI.Models;

namespace TrueStoryAPI.Repositories
{
    /// <summary>
    /// Repository for managing Product entities via HTTP API calls.
    /// Implements IProductRepository to provide CRUD operations.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="httpClient">Injected HttpClient for making API requests.</param>
        public ProductRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves a paginated list of products, optionally filtered by name.
        /// </summary>
        /// <param name="name">Optional name filter (case-insensitive substring match).</param>
        /// <param name="page">Page number (1-based).</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>Enumerable of products for the specified page and filter.</returns>
        public async Task<IEnumerable<Product>> GetAllAsync(string? name, int page, int pageSize)
        {
            // Fetch all products from the API endpoint
            var products = await _httpClient.GetFromJsonAsync<List<Product>>("/objects")
                           ?? new List<Product>();
            // Filter by name if provided
            if (!string.IsNullOrWhiteSpace(name))
            {
                products = products
                    .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            // Apply pagination
            return products
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        /// <summary>
        /// Retrieves products by a collection of IDs.
        /// </summary>
        /// <param name="ids">Enumerable of product IDs.</param>
        /// <returns>Enumerable of products matching the given IDs.</returns>
        public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<string> ids)
        {
            // Build up one (key,value) pair for each “id=…” fragment
            var queryParams = ids
               .Select(id => new KeyValuePair<string, string?>("id", id));

            // This overload will produce “/objects?id=3&id=5&id=10…”
            var uri = QueryHelpers.AddQueryString("/objects", queryParams);

            // Fetch products by IDs from the API
            var products = await _httpClient
               .GetFromJsonAsync<List<Product>>(uri);

            return products ?? Enumerable.Empty<Product>();
        }

        /// <summary>
        /// Retrieves a single product by its ID.
        /// </summary>
        /// <param name="id">Product ID.</param>
        /// <returns>The product if found; otherwise, null.</returns>
        public async Task<Product?> GetByIdAsync(string id)
        {
            try
            {
                // Fetch product by ID from the API
                return await _httpClient.GetFromJsonAsync<Product>($"/objects/{id}");
            }
            catch (HttpRequestException)
            {
                // Return null if not found or request fails
                return null;
            }
        }

        /// <summary>
        /// Creates a new product using the provided DTO.
        /// </summary>
        /// <param name="dto">DTO containing product creation data.</param>
        /// <returns>The created product.</returns>
        /// <exception cref="Exception">Thrown if product creation fails.</exception>
        public async Task<Product> CreateAsync(CreateProductDto dto)
        {
            // Send POST request to create a new product
            var response = await _httpClient.PostAsJsonAsync("/objects", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Product>()
                   ?? throw new Exception("Failed to create product.");
        }

        /// <summary>
        /// Updates an existing product by ID using the provided DTO.
        /// </summary>
        /// <param name="id">Product ID.</param>
        /// <param name="dto">DTO containing updated product data.</param>
        /// <returns>The updated product.</returns>
        /// <exception cref="Exception">Thrown if product update fails.</exception>
        public async Task<Product> UpdateAsync(string id, UpdateProductDto dto)
        {
            // Send PUT request to update the product
            var response = await _httpClient.PutAsJsonAsync($"/objects/{id}", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Product>()
                   ?? throw new Exception("Failed to update product.");
        }

        /// <summary>
        /// Partially updates a product by ID using the provided patch DTO.
        /// </summary>
        /// <param name="id">Product ID.</param>
        /// <param name="dto">DTO containing patch data.</param>
        /// <returns>The patched product.</returns>
        /// <exception cref="Exception">Thrown if patch operation fails.</exception>
        public async Task<Product> PatchAsync(string id, PatchProductDto dto)
        {
            // Create PATCH request for partial update
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/objects/{id}")
            {
                Content = JsonContent.Create(dto)
            };
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Product>()
                   ?? throw new Exception("Failed to patch product.");
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">Product ID.</param>
        /// <returns>True if deletion was successful; otherwise, false.</returns>
        public async Task<bool> DeleteAsync(string id)
        {
            // Send DELETE request to remove the product
            var response = await _httpClient.DeleteAsync($"/objects/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
