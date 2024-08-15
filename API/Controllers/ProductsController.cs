// Imports
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// File path
namespace API.Controllers;

// This is needed to create a API controller and its route
// Gives routes automatic model binding
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase 
{
    // Store context variable
    private readonly StoreContext context;

    // Assings the store context to a global variable
    public ProductsController(StoreContext context)
    {
        this.context = context;
    }

    // Get products api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts() 
    {
        // Gets all the products from the database
        return await context.Products.ToListAsync();
    }

    // Get products api/products/2
    [HttpGet("{id:int}")] 
    public async Task<ActionResult<Product>> GetProduct(int id) 
    {
        // Gets a single product from the database
        var product = await context.Products.FindAsync(id);

        // If product does not exists, return not found
        if(product == null) return NotFound();

        // Returns product
        return product;
    }

    // Create product api/products
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product) 
    {
        // Creates product and adds it to the database
        context.Products.Add(product);

        // Saves the database changes
        await context.SaveChangesAsync();

        // Returns the product
        return product;
    }

    // Updates a product api/products/2
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product) 
    {
        // Checks if the id param and the id of the product are the same, and if a product has that id
        // If so, return a bad request
        if(product.Id != id || !ProductExists(id)) 
            return BadRequest("Cannot update this request");

        // Tell entity framework that the product was modifies so it tracks its modified state
        context.Entry(product).State = EntityState.Modified;

        // Saves the database changes
        await context.SaveChangesAsync();

        // Return nothing
        return NoContent();
    }

    // Deletes a product api/products/2
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        // Gets the product with the given id
        var product = await context.Products.FindAsync(id);

        // If the product doesn't exist, return not found
        if(product == null) return NotFound();

        // Tell entity framework to remove product
        context.Products.Remove(product);

        // Saves the database changes
        await context.SaveChangesAsync();

        // Return nothing
        return NoContent();
    }

    // Checks if a product has the given id
    private bool ProductExists(int id)
    {
        return context.Products.Any(x => x.Id == id);
    }
}
