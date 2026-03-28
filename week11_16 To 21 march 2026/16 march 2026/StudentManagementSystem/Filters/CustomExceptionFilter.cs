using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StudentManagementSystem.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ContentResult
            {
                Content = "Something went wrong!",
                StatusCode = 500
            };

            context.ExceptionHandled = true;
        }
    }
}