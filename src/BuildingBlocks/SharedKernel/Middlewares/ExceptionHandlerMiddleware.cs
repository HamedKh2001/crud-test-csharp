﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedKernel.Exceptions;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SharedKernel.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ConvertException(context, ex);
            }
        }

        private Task ConvertException(HttpContext context, Exception exception)
        {
            int httpStatusCode;
            string result = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    httpStatusCode = (int)HttpStatusCode.BadRequest;
                    result = string.Join(Environment.NewLine, (exception as ValidationException).Errors.Select(x => x.Value[0]));
                    break;
                case BadRequestException badRequestException:
                    httpStatusCode = (int)HttpStatusCode.BadRequest;
                    result = badRequestException.Message;
                    break;
                case NotFoundException:
                    httpStatusCode = (int)HttpStatusCode.NotFound;
                    result = exception.Message;
                    break;
                case ApiException apiException:
                    httpStatusCode = (int)HttpStatusCode.InternalServerError;
                    result = apiException.Message;
                    break;
                default:
                    httpStatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }


            _logger.LogError(exception, exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = httpStatusCode;

            if (string.IsNullOrEmpty(result))
                result = exception.Message;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new { StatusCode = httpStatusCode, error = result }));
        }
    }
}
