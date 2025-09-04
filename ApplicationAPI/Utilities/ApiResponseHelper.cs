using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Structure_Core.BaseClass;

namespace ApplicationAPI.Utilities;

public static class ApiResponseHelper
{
    public static IActionResult HandleResult<T>(ResultService<T> result, ControllerBase controller)
    {
        return result.Code switch
        {
            "0" => controller.Ok(result),
            "1" => controller.BadRequest(result),
            _ => controller.NotFound(result)
        };
    }
}
