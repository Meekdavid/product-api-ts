using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using TrueStoryAPI.DTOs;
using TrueStoryAPI.Interfaces;

namespace TrueStoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository repo, ILogger<ProductsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a paginated list of products.
        /// </summary>
        /// <remarks>
        /// Fetches products from the mock API, supports optional filtering by name substring
        /// and paging via page (starting at 1) and pageSize (default is 10).
        /// </remarks>
        /// <param name="name">Optional substring to filter products by name (case-insensitive).</param>
        /// <param name="page">Page number (starting from 1).</param>
        /// <param name="pageSize">Number of items per page.</param>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? name,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Page and pageSize must be greater than zero.");

            try
            {
                var items = await _repo.GetAllAsync(name, page, pageSize);
                return Ok(items);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching product list.");
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "External service unavailable.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching product list.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Retrieves multiple products by their IDs.
        /// </summary>
        /// <remarks>
        /// Fetches the specified products from the mock API by supplying one or more id query parameters.
        /// </remarks>
        /// <param name="id">Array of product IDs to retrieve (e.g., ?id=1&amp;id=2).</param>
        [HttpGet("batch")]
        public async Task<IActionResult> GetByIds([FromQuery] string[] id)
        {
            if (id == null || id.Length == 0)
                return BadRequest("At least one id must be provided.");

            try
            {
                var items = await _repo.GetByIdsAsync(id);
                return Ok(items);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching products for ids: {Ids}", string.Join(", ", id));
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "External service unavailable.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching products for ids: {Ids}", string.Join(", ", id));
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Retrieves a single product by ID.
        /// </summary>
        /// <remarks>
        /// Fetches one product from the mock API. Returns 404 if not found.
        /// </remarks>
        /// <param name="id">The ID of the product to retrieve.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id must be provided.");

            try
            {
                var product = await _repo.GetByIdAsync(id);
                if (product == null)
                    return NotFound();
                return Ok(product);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching product with id: {Id}", id);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "External service unavailable.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching product with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <remarks>
        /// Posts a new product to the mock API. The request body must include a valid CreateProductDto with Name and optional Data.
        /// </remarks>
        /// <param name="dto">The product data transfer object containing Name and optional Data.</param>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _repo.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error creating product.");
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "External service unavailable.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating product.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Updates an existing product by replacing it entirely.
        /// </summary>
        /// <remarks>
        /// Sends a full UpdateProductDto to replace the product at the specified ID. Returns the updated product.
        /// </remarks>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="dto">The full product DTO to use for replacement.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id must be provided.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _repo.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error updating product with id: {Id}", id);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "External service unavailable.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating product with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Partially updates a product.
        /// </summary>
        /// <remarks>
        /// Sends a PatchProductDto to apply partial updates to the product with the given ID. Returns the patched product.
        /// </remarks>
        /// <param name="id">The ID of the product to patch.</param>
        /// <param name="dto">The partial DTO containing only fields to update.</param>
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(string id, [FromBody] PatchProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id must be provided.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var patched = await _repo.PatchAsync(id, dto);
                return Ok(patched);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error patching product with id: {Id}", id);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "External service unavailable.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error patching product with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <remarks>
        /// Deletes the specified product from the mock API. Returns 204 NoContent on success or 404 if not found.
        /// </remarks>
        /// <param name="id">The ID of the product to delete.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id must be provided.");

            try
            {
                var success = await _repo.DeleteAsync(id);
                if (!success)
                    return NotFound();
                return NoContent();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error deleting product with id: {Id}", id);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "External service unavailable.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting product with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}