using Microsoft.AspNetCore.Mvc;
using Structure_Core.UserManagement;
using Structure_Interface.IBaseServices;
using Structure_Interface.IUserService;

namespace ApplicationAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ICRUD_Service<User, string> _service;
    private readonly IUserProvider _userProvider;

    public UserController(ICRUD_Service<User, string> service, IUserProvider userProvider)
    {
        _service = service;
        _userProvider = userProvider;

    }

    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAll();
        return result.Code == "0" ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _service.Get(id);
        return result.Code == "0" ? Ok(result) : NotFound(result);

    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create(User user)
    {
        var result = await _service.Create(user);
        return result.Code == "0" ? Ok(result) : NotFound(result);
    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update(User user)
    {
        var result = await _service.Update(user);
        return result.Code == "0" ? Ok(result) : NotFound(result);

    }

    [HttpDelete("{id}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _service.Delete(id);
        return result.Code == "0"? Ok(result) : NotFound(result) ;
    }
    [HttpGet("MongoSearch")]
    public async Task<IActionResult> MongoSearch([FromQuery] string? keyword = null)
    {
        var result = await _userProvider.MongoSearch(keyword);
        return result.Code == "0" ? Ok(result) : NotFound(result);
    }
    [HttpGet("LinqSearch")]
    public async Task<IActionResult> LinqSearch([FromQuery] string? keyword = null, [FromQuery] string? orderByDescending = null)
    {
        var result = await _userProvider.LinqSearch(keyword, orderByDescending);
        return result.Code == "0" ? Ok(result) : NotFound(result);
    }

    [HttpGet("FilterByDate")]
    public async Task<IActionResult> FilterByDate([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        var result = await _userProvider.FilterDateRange(startDate: startDate, endDate: endDate);
        return result.Code == "0" ? Ok(result) : NotFound(result);
    }

    [HttpGet("Search")]
    public async Task<IActionResult> Search(
    [FromQuery] string? keyword = null,
    [FromQuery] string? orderBy = null,
    [FromQuery] DateTime? startDate = null,
    [FromQuery] DateTime? endDate = null)
    {
        var result = await _userProvider.Search(keyword, orderBy, startDate, endDate);
        return result.Code == "0" ? Ok(result) : NotFound(result);
    }
    [HttpGet("FilterByProduct")]
    public async Task<IActionResult> FilterByProduct([FromQuery] string productId)
    {
        var result = await _userProvider.FilterUsersByProduct(productId);
        return result.Code == "0" ? Ok(result) : NotFound(result);
    }
}