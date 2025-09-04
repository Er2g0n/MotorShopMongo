using Microsoft.AspNetCore.Mvc;
using Structure_Core.UserManagement;
using Structure_Interface.IBaseServices;
using Structure_Interface.IUserService;
using static ApplicationAPI.Utilities.ApiResponseHelper;

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
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await _service.GetAll(), this);
    }

    [HttpGet("{ID}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetById(string ID)
    {
        return HandleResult(await _service.Get(ID), this);
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Create(User user)
    {
        return HandleResult(await _service.Create(user), this);
    }

    [HttpPut]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Update(User user)
    {
        return HandleResult(await _service.Update(user), this);
    }

    [HttpDelete("{ID}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Delete(string ID)
    {
        return HandleResult(await _service.Delete(ID), this);
    }
    [HttpGet("MongoSearch")]
    public async Task<IActionResult> MongoSearch([FromQuery] string? keyword = null)
    {
        return HandleResult(await _userProvider.MongoSearch(keyword), this);
    }
    [HttpGet("LinqSearch")]
    public async Task<IActionResult> LinqSearch([FromQuery] string? keyword = null, [FromQuery] string? orderBy = null)
    {
        return HandleResult(await _userProvider.LinqSearch(keyword, orderBy), this);
    }

    [HttpGet("FilterByDate")]
    public async Task<IActionResult> FilterByDate([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        return HandleResult(await _userProvider.FilterDateRange(startDate: startDate, endDate: endDate), this);
    }

    [HttpGet("Search")]
    public async Task<IActionResult> Search(
    [FromQuery] string? orderBy = null,
    [FromQuery] string? keyword = null,
    [FromQuery] DateTime? startDate = null,
    [FromQuery] DateTime? endDate = null)
    {
    
        return HandleResult(await _userProvider.Search(keyword, orderBy, startDate, endDate), this);
    }
    [HttpGet("FilterByProduct")]
    public async Task<IActionResult> FilterByProduct([FromQuery] string productId)
    {
        return HandleResult(await _userProvider.FilterUsersByProduct(productId), this);
    }
}