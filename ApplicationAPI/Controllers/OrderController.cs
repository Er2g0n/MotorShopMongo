using Microsoft.AspNetCore.Mvc;
using Structure_Core.TransactionManagement;
using Structure_Interface.IBaseServices;
using Structure_Interface.ITransactionService;

namespace ApplicationAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ICRUD_Service<Order, string> _service;
    private readonly IOrderProvider _orderProvider;
    public OrderController(ICRUD_Service<Order,string> service, IOrderProvider orderProvider )
    {
        _service = service;
        _orderProvider = orderProvider;
    }
    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAll();
        return result == null ? BadRequest("No order found") : Ok(result);
    }

    [HttpGet("{id}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _service.Get(id);
        if (result.Data == null)
            return NotFound(result);
        return Ok(result);
    }
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] Order order)
    {
        var result = await _service.Create(order);
        return Ok(result);
    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update( [FromBody] Order order)
    {
        var result = await _service.Update(order);
        if (result.Data == null)
            return NotFound(result);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _service.Delete(id);
        if (result.Data == null)
            return NotFound(result);

        return Ok(result);
    }
    [HttpGet("Search")]
        public async Task<IActionResult> Search(
        [FromQuery] string? keyword = null,
        [FromQuery] string? orderBy = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
            {
        var result = await _orderProvider.Search(keyword, orderBy, startDate, endDate);
        return result.Code == "200" ? Ok(result) : NotFound(result);
    }

}
