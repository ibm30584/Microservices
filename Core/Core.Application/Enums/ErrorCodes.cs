﻿namespace Core.Application.Enums
{
    public enum ErrorCodes
    {
        None = 0,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500,
        BadGateway = 502,
        ServiceUnavailable = 503
    }
}
