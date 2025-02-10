using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// Defines an API controller for managing products
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> repo) : BaseApiController
{
    // Retrieves all products, with optional filtering by brand, type, and sorting
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
        [FromQuery] ProductSpecParams specParams
    )
    {
        var spec = new ProductSpecification(specParams);

        return await CreatePageResult(repo, spec, specParams.PageIndex, specParams.PageSize);
    }

    // Retrieves a specific product by ID
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);
        return product == null ? NotFound() : Ok(product);
    }

    // Adds a new product
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        repo.Add(product);
        if (await repo.SaveAllAsync())
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);

        return BadRequest("Problem creating product");
    }

    // Updates an existing product by ID
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !ProductExists(id))
            return BadRequest("Cannot update this product");

        repo.Update(product);
        if (await repo.SaveAllAsync())
            return NoContent();

        return BadRequest("Problem updating the product");
    }

    // Deletes a product by ID
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        repo.Remove(product);
        if (await repo.SaveAllAsync())
            return NoContent();

        return BadRequest("Problem deleting the product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        var brands = await repo.ListAsync(spec);
        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();
        var types = await repo.ListAsync(spec);
        return Ok(types);
    }

    // Checks if a product exists in the database by its ID
    private bool ProductExists(int id) => repo.Exists(id);
}
