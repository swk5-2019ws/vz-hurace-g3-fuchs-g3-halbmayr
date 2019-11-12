using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hurace.Core.Db
{
    public class SimpleSqlQueryGenerator
    {
        private IEnumerable<string> SkipableProperties { get; }

        public SimpleSqlQueryGenerator(IEnumerable<string> skipableProperties)
        {
            SkipableProperties = skipableProperties;
        }

        public string GenerateGetAllSelectQuery<T>()
        {
            var sb = new StringBuilder();

            sb.Append("SELECT");
            bool firstProperty = true;
            foreach (var currentProperty in typeof(T).GetProperties())
            {
                if (!SkipableProperties.Any(p => p == currentProperty.Name))
                {
                    sb.Append($"{(firstProperty ? "" : ",")} [{currentProperty.Name}]");
                    firstProperty = false;
                }
            }

            sb.Append($" FROM [Hurace].[{typeof(T).Name}]");

            return sb.ToString();
        }
    }
}
