using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Hurace.Core.Logging.Extensions
{
    public static class ILoggerExtensions
    {
        public static void LogCall(
            this ILogger<object> logger,
            object callingMethodParameters = null,
            [CallerMemberName]string callingMethodName = "")
        {
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));
            else if (string.IsNullOrEmpty(callingMethodName))
                throw new ArgumentException($"{callingMethodName} is null or empty", nameof(callingMethodName));

            var informationMessageBuilder = new StringBuilder();

            informationMessageBuilder.Append($"{callingMethodName} called");

            if (callingMethodParameters != null)
            {
                informationMessageBuilder.Append(" with (");
                foreach (var property in callingMethodParameters.GetType().GetProperties())
                {
                    informationMessageBuilder.Append($" '{property.Name}: {property.GetValue(callingMethodParameters)}'");
                }
                informationMessageBuilder.Append(" )");
            }

            logger.LogInformation(informationMessageBuilder.ToString());
        }
    }
}
