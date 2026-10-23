using Microsoft.AspNetCore.Mvc;
using BankProductsAPI.Attributes;
using BankProductsAPI.Models;
using BankProductsAPI.Services;

namespace BankProductsAPI.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _products;

    public ProductsController(ProductService products)
    {
        _products = products;
    }

    [HttpGet]
    [BasicAuthentication]
    [MyAuthorize("Admin", "Manager", "Client")]
    public ActionResult<List<BankProduct>> GetAll()
    {
        return Ok(_products.GetAll());
    }

    [HttpGet("{id:int}")]
    [BasicAuthentication]
    [MyAuthorize("Admin", "Manager", "Client")]
    public ActionResult<BankProduct> GetById(int id)
    {
        var product = _products.Get(id);
        if (product == null)
            return NotFound(new { message = $"Продукт с id={id} не найден" });

        return Ok(product);
    }

    [HttpPost]
    [BasicAuthentication]
    [MyAuthorize("Admin", "Manager")]
    public IActionResult Create(BankProduct product)
    {
        var created = _products.Add(product);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [BasicAuthentication]
    [MyAuthorize("Admin", "Manager")]
    public IActionResult Update(int id, BankProduct product)
    {
        var updated = _products.Update(id, product);
        if (updated == null)
            return NotFound(new { message = $"Продукт с id={id} не найден" });

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [BasicAuthentication]
    [MyAuthorize("Admin")]
    public IActionResult Delete(int id)
    {
        if (!_products.Delete(id))
            return NotFound(new { message = $"Продукт с id={id} не найден" });

        return NoContent();
    }
}
