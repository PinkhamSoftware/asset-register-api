using System;
using System.Collections.Generic;
using System.Threading;
using HomesEngland.UseCase.SearchAsset.Models;
using Infrastructure.Api.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace WebApi.Extensions
{
    public class ResponseData<T>
    {
        public T Data { get; }

        public ResponseData(T data)
        {
            Data = data;
        }
    }

    public static class ControllerExtensions
    {
        public static IActionResult StandardiseResponse<TResponse, TData>(this ControllerBase controller,
            TResponse useCaseResult) where TResponse : IResponse<TData>
        {
            if (controller?.Request != null &&
                controller.ControllerContext.HttpContext.Request.Headers.Contains(
                    new KeyValuePair<string, StringValues>("accept", "text/csv")))
            {
                controller.ControllerContext.HttpContext?.Response?.Headers.Add("Content-Disposition",
                    "attachment; filename=export.csv;");
                return controller.StatusCode(200, useCaseResult?.ToCsv());
            }


            return controller?.StatusCode(controller.Response?.StatusCode ?? 400,
                new ResponseData<TResponse>(useCaseResult));
        }


        public static CancellationToken GetCancellationToken(this ControllerBase controller)
        {
            if (controller?.HttpContext == null)
            {
                return new CancellationToken();
            }

            return controller.HttpContext.RequestAborted;
        }
    }
}
