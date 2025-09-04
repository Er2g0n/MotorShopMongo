using Microsoft.AspNetCore.Mvc;
using Structure_Core.TransactionManagement;
using Structure_Interface.IBaseServices;
using Structure_Interface.ITransactionService;
using static ApplicationAPI.Utilities.ApiResponseHelper;

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
    [Produces("application/json")]
    public async Task<IActionResult> GetAll() => HandleResult(await _service.GetAll(), this);
    

    [HttpGet("{ID}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(string ID)
    {
        var result = await _service.Get(ID);
        if (result.Data == null)
            return NotFound(result);
        return HandleResult(result, this);
    }
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] Order order)
    {
        var result = await _service.Create(order);
        return HandleResult(result, this);
    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update( [FromBody] Order order)
    {
        var result = await _service.Update(order);
        if (result.Data == null)
            return NotFound(result);
        return HandleResult(result, this);
    }

    [HttpDelete("{ID}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(string ID)
    {
        var result = await _service.Delete(ID);
        if (result.Data == null)
            return NotFound(result);
        return HandleResult(result, this);
    }
    [HttpGet("Search")]
        public async Task<IActionResult> Search(
        [FromQuery] string? keyword = null,
        [FromQuery] string? orderBy = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
            {
        var result = await _orderProvider.Search(keyword, orderBy, startDate, endDate);
        return HandleResult(result, this);
    }

}
