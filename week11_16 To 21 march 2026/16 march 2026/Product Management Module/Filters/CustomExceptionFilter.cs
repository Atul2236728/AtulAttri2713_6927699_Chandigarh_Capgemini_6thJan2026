using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Product_Management_Module.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Console.WriteLine("[ERROR] Exception Occurred");
            Console.WriteLine(context.Exception.Message);

            context.Result = new ContentResult
            {
                Content = "Something went wrong. Please try again later.",
                StatusCode = 500
            };

            context.ExceptionHandled = true;
        }
    }
}