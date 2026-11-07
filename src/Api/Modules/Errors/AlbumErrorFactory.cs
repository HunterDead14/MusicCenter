using Application.Albums.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class AlbumErrorFactory
{
    public static ObjectResult ToObjectResult(this AlbumException error)
    {
        return new ObjectResult(error.Message)
        {
            StatusCode = error switch
            {
                AlbumAlreadyExistsException => StatusCodes.Status409Conflict,
                AlbumNotFoundException => StatusCodes.Status404NotFound,
                AlbumUnhandledException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Album error handler is not implemented.")
            }
        };
    }
}
