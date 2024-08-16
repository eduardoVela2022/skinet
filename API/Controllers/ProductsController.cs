// Imports
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// File path
namespace API.Controllers;

// This is needed to create a API controller and its route
// Gives routes automatic model binding
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository repo) : ControllerBase 
{
    // Get products api/products
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, 
    string? type, string? sort) // Query params (If these parameters are string, the api controller will search the query params for them)
    {
        // Gets all the products from the database
        return Ok(await repo.GetProductsAsync(brand, type, sort));
    }

    // Get products api/products/2
    [HttpGet("{id:int}")] 
    public async Task<ActionResult<Product>> GetProduct(int id) 
    {
        // Gets a single product from the database
        var product = await repo.GetProductByIdAsync(id);

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
        repo.AddProduct(product);

        // Saves the database changes and returns the product if it was successfull, also returns the products location
        if(await repo.SavaChangesAsync()) {
            return CreatedAtAction("GetProduct", new {id = product.Id}, product);
        }

        // If the product was not saved successfullly return bad request
        return BadRequest("Problem creating the product");
    }

    // Updates a product api/products/2
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product) 
    {
        // Checks if the id param and the id of the product are the same, and if a product has that id
        // If so, return a bad request
        if(product.Id != id || !ProductExists(id)) 
            return BadRequest("Cannot update this product");

        // Tell entity framework that the product was modifies so it tracks its modified state
        repo.UpdateProduct(product);

        // Saves the database changes and return nothing
        if(await repo.SavaChangesAsync()) 
        {
            return NoContent();
        }

        // If the product was not updated successfullly return bad request
        return BadRequest("Problem updating the product");
    }

    // Deletes a product api/products/2
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        // Gets the product with the given id
        var product = await repo.GetProductByIdAsync(id);

        // If the product doesn't exist, return not found
        if(product == null) return NotFound();

        // Tell entity framework to remove product
        repo.DeleteProduct(product);

        // Saves the database changes and return nothing
        if(await repo.SavaChangesAsync()) 
        {
            return NoContent();
        }

        // If the product was not deleted successfullly return bad request
        return BadRequest("Problem deleting the product");
    }

    // Deletes a product api/products/brands
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands() 
    {
        return Ok(await repo.GetBrandsAsync());
    }

    // Deletes a product api/products/types
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes() 
    {
        return Ok(await repo.GetTypesAsync());
    }

    // Checks if a product has the given id
    private bool ProductExists(int id)
    {
        return repo.ProductExists(id);
    }
}
