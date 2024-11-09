using Core.Entites;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly StoreContext context;

    public ProductsController(StoreContext context)
    {
        this.context = context;
    }

    // GET api/products
    // This method retrieves a list of all products from the database.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts() 
    {
        return await context.Products.ToListAsync();
    }

    // GET api/products/{id}
    // This method retrieves a specific product by its ID from the database.
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id) 
    {
       var product = await context.Products.FindAsync(id);

       if(product == null) return NotFound();

       return product;
    }

    // POST api/products
    // This method adds a new product to the database.
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }

    // PUT api/products/{id}
    // This method updates an existing product in the database.
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !ProductExists(id))
            return BadRequest("Cannot update this product");

        context.Entry(product).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE api/products/{id}
    // This method deletes a product from the database.
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        context.Products.Remove(product);
        await context.SaveChangesAsync();

        return NoContent();
    }

    // This helper method checks if a product exists in the database by its ID.
    private bool ProductExists(int id)
    {
        return context.Products.Any(x => x.Id == id);
    }
}
