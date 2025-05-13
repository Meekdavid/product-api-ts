using TrueStoryAPI.DTOs;
using TrueStoryAPI.Models;

namespace TrueStoryAPI.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(string? name, int page, int pageSize);
        Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<string> ids);
        Task<Product?> GetByIdAsync(string id);
        Task<Product> CreateAsync(CreateProductDto dto);
        Task<Product> UpdateAsync(string id, UpdateProductDto dto);
        Task<Product> PatchAsync(string id, PatchProductDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
