using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace StudentManagementSystem.Filters
{
    public class LogActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"[LOG] {context.ActionDescriptor.DisplayName}");
            Console.WriteLine($"Time: {DateTime.Now}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("[LOG] Action Completed");
        }
    }
}