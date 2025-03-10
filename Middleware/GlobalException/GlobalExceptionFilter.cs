using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.GlobalException
{
    public  class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            var errorResponse = ExceptionHandler.HandleException(context.Exception, _logger);

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = 500
            };

            context.ExceptionHandled = true;
        }

       
    }
}
