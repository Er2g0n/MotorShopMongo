using Microsoft.AspNetCore.Mvc;
using Structure_Core.ProductManagement;
using Structure_Interface.IBaseServices;

namespace ApplicationAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ICRUD_Service<Product, string> _service;
    public ProductController(ICRUD_Service<Product, string> service)
    {
        _service = service;
    }
    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAll();
        return result == null ? BadRequest("No products found") : Ok(result);
    }
}
