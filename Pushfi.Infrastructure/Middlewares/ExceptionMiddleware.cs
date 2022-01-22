﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pushfi.Domain.Exceptions;
using Pushfi.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security;

namespace Pushfi.Infrastructure.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate next;
		private readonly IWebHostEnvironment env;
		private readonly ILogger<ExceptionMiddleware> logger;

		public ExceptionMiddleware(
			RequestDelegate next,
			IWebHostEnvironment env,
			ILoggerFactory loggerFactory)
		{
			this.next = next ?? throw new ArgumentNullException(nameof(next));
			this.env = env ?? throw new ArgumentNullException(nameof(env));
			logger = loggerFactory?.CreateLogger<ExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				LogCriticalException(ex);
				await HandleExceptionAsync(context, ex);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var jsonSettings = new JsonSerializerSettings()
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			};
			var error = new ErrorModel()
			{
				Message = exception.Message,
				ExceptionType = env.IsDevelopment() ? exception.GetType().FullName : null,
				StackTrace = env.IsDevelopment() ? exception.StackTrace : null
			};
			var result = JsonConvert.SerializeObject(error, jsonSettings);

			context.Response.Clear();
			context.Response.StatusCode = GetHttpStatusCode(exception);
			context.Response.ContentType = "application/json";

			return context.Response.WriteAsync(result);
		}

		private void LogCriticalException(Exception ex)
		{
			logger.LogCritical(ex, ex.InnerException != null
				? ex.InnerException.Message
				: ex.Message);
		}

		private int GetHttpStatusCode(Exception exception)
		{
			int result;

			switch (exception)
			{
				case InvalidModelStateException _:
				case ValidationException _:
				case BusinessException _:
					result = (int)HttpStatusCode.BadRequest;
					break;
				case SecurityException _:
				case SecurityTokenExpiredException _:
					result = (int)HttpStatusCode.Unauthorized;
					break;
				case EntityNotFoundException _:
					result = (int)HttpStatusCode.NotFound;
					break;
				case MethodAccessException _:
					result = (int)HttpStatusCode.MethodNotAllowed;
					break;
				case EntityNotUniqueException _:
					result = (int)HttpStatusCode.Conflict;
					break;
				default:
					result = (int)HttpStatusCode.InternalServerError;
					break;
			}

			return result;
		}
	}
}
