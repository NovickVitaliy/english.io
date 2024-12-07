using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Shared.ErrorHandling;

public class Result<T>
{
    public string Description { get; }
    public HttpStatusCode StatusCode { get; }
    public T Data { get; }
    public string? Location { get; }

    private Result(string description, HttpStatusCode statusCode, T data, string? location = null)
    {
        Description = description;
        StatusCode = statusCode;
        Data = data;
        Location = location;
    }

    public static Result<T> Ok(T data)
    {
        return new Result<T>(ErrorMessages.NoError(), HttpStatusCode.OK, data);
    }

    public static Result<T> NoContent()
    {
        return Ok(default!);
    }

    public static Result<T> Created(string location, T value)
    {
        return new Result<T>(ErrorMessages.NoError(), HttpStatusCode.Created, value, location);
    }

    public static Result<T> BadRequest(string reason)
    {
        return new Result<T>(ErrorMessages.BadRequest(reason), HttpStatusCode.BadRequest, default!);
    }

    public static Result<T> NotFound(object key)
    {
        return new Result<T>(ErrorMessages.NotFound<T>(key), HttpStatusCode.NotFound, default!);
    }

    public IActionResult ToApiResponse()
    {
        return StatusCode switch
        {
            HttpStatusCode.OK => new OkObjectResult(Data),
            HttpStatusCode.NoContent => new NoContentResult(),
            HttpStatusCode.Created => new CreatedResult(Location, Data),
            HttpStatusCode.BadRequest => new BadRequestObjectResult(this),
            HttpStatusCode.NotFound => new NotFoundObjectResult(this),
            _ => new ObjectResult(this) { StatusCode = (int?)HttpStatusCode.InternalServerError }
        };
    }
}