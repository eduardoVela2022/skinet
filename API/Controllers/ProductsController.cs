// Imports
using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

// File path
namespace API.Controllers;

// This is needed to create a API controller and its route
// Gives routes automatic model binding
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IUnitOfWork unit) : BaseApiController
{
    // Get products api/products
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
        [FromQuery] ProductSpecParams specParams
    ) // Query params (If these parameters are string, the api controller will search the query params for them)
    {
        // Creates optional specs
        var spec = new ProductSpecification(specParams);

        // Returns products
        return await CreatePagedResult(
            unit.Repository<Product>(),
            spec,
            specParams.PageIndex,
            specParams.PageSize
        );
    }

    // Get products api/products/2
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        // Gets a single product from the database
        var product = await unit.Repository<Product>().GetByIdAsync(id);

        // If product does not exists, return not found
        if (product == null)
            return NotFound();

        // Returns product
        return product;
    }

    // Create product api/products
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        // Creates product and adds it to the database
        unit.Repository<Product>().Add(product);

        // Saves the database changes and returns the product if it was successfull, also returns the products location
        if (await unit.Complete())
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
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
        if (product.Id != id || !ProductExists(id))
            return BadRequest("Cannot update this product");

        // Tell entity framework that the product was modifies so it tracks its modified state
        unit.Repository<Product>().Update(product);

        // Saves the database changes and return nothing
        if (await unit.Complete())
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
        var product = await unit.Repository<Product>().GetByIdAsync(id);

        // If the product doesn't exist, return not found
        if (product == null)
            return NotFound();

        // Tell entity framework to remove product
        unit.Repository<Product>().Remove(product);

        // Saves the database changes and return nothing
        if (await unit.Complete())
        {
            return NoContent();
        }

        // If the product was not deleted successfullly return bad request
        return BadRequest("Problem deleting the product");
    }

    // Gets the brands of a product product api/products/brands
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();

        return Ok(await unit.Repository<Product>().ListAsync(spec));
    }

    // Deletes the types of a product api/products/types
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();

        return Ok(await unit.Repository<Product>().ListAsync(spec));
    }

    // Checks if a product has the given id
    private bool ProductExists(int id)
    {
        return unit.Repository<Product>().Exists(id);
    }
}
