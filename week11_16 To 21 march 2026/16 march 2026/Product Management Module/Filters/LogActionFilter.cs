using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Product_Management_Module.Filters
{
    public class LogActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var actionName = context.ActionDescriptor.DisplayName;

            Console.WriteLine($"[LOG] Action Executing: {actionName}");
            Console.WriteLine($"[LOG] Time: {DateTime.Now}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("[LOG] Action Finished");
        }
    }
}