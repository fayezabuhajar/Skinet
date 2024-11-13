using Core.Entites;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository repo) : ControllerBase
{
   

    // GET api/products
    // This method retrieves a list of all products from the database.
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand,
     string? type,string? sort) 
    {
        return Ok(await repo.GetProductsAsync(brand, type, sort));
    }

    // GET api/products/{id}
    // This method retrieves a specific product by its ID from the database.
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id) 
    {
        var product = await repo.GetProductByIdAsync(id);

       if(product == null) return NotFound();

       return product;
    }

    // POST api/products
    // This method adds a new product to the database.
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        repo.AddProduct(product);
       if(await repo.SaveChangesAsync())
       {
        return CreatedAtAction("GetProduct",new {id =product.Id}, product);
        }

        return BadRequest("Problem creating product");
    }

    // PUT api/products/{id}
    // This method updates an existing product in the database.
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !ProductExists(id))
            return BadRequest("Cannot update this product");

       repo.UpdateProduct(product);
        if(await repo.SaveChangesAsync())
        {
            return NoContent();
        }


        return BadRequest("Problem updating the product");
    }

    // DELETE api/products/{id}
    // This method deletes a product from the database.
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);

        if (product == null) return NotFound();

        repo.DeleteProduct(product);
        
          if(await repo.SaveChangesAsync())
        {
            return NoContent();
        }


        return BadRequest("Problem deleting the product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return  Ok( await repo.GetBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTyoes()
    {
        return  Ok(await repo.GetTypesAsync());
    }




    // This helper method checks if a product exists in the database by its ID.
    private bool ProductExists(int id)
    {
        return repo.ProductExists(id);
    }
}
