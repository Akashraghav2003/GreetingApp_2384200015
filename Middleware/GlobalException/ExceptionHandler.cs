//using NLog;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.GlobalException
{
    public class ExceptionHandler
    {
        public static object HandleException(Exception ex, ILogger logger)
        {
            logger.LogError(ex, "An unexpected error occurred.");

            return new
            {
                Success = false,
                Message = "An unexpected error occurred.",
                ErrorDetails = ex.Message
            };
        }
    }
}
