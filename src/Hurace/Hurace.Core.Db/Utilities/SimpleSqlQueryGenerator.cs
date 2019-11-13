using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hurace.Core.Db.Utilities
{
    public class SimpleSqlQueryGenerator
    {
        private IEnumerable<string> SkipableProperties { get; }

        public SimpleSqlQueryGenerator(IEnumerable<string> skipableProperties)
        {
            SkipableProperties = skipableProperties;
        }

        private void AppendDbColumns<T>(StringBuilder sb)
        {
            bool firstProperty = true;
            foreach (var currentProperty in typeof(T).GetProperties())
            {
                if (SkipableProperties != null && !SkipableProperties.Any(p => p == currentProperty.Name))
                {
                    sb.Append($"{(firstProperty ? "" : ",")} [{currentProperty.Name}]");
                    firstProperty = false;
                }
            }
        }

        public string GenerateGetAllQuery<T>()
        {
            var sb = new StringBuilder();

            sb.Append("SELECT");
            AppendDbColumns<T>(sb);

            sb.Append($" FROM [Hurace].[{typeof(T).Name}]");

            return sb.ToString();
        }

        public string GenerateGetByIdQuery<T>(int id)
        {
            var sb = new StringBuilder();

            sb.Append("SELECT");
            AppendDbColumns<T>(sb);

            sb.Append($" FROM [Hurace].[{typeof(T).Name}]");
            sb.Append($" WHERE Id = {id}");

            return sb.ToString();
        }

        public string GenerateCreateQuery()
        {
            throw new NotImplementedException();
        }

        public string GenerateUpdateQuery()
        {
            throw new NotImplementedException();
        }

        public string GenerateDeleteByIdQuery()
        {
            throw new NotImplementedException();
        }

    }
}
