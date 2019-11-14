using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hurace.Core.Db.Utilities
{
    public class SimpleSqlQueryGenerator<T>
    {
        public string GenerateGetAllQuery()
        {
            var sb = new StringBuilder();

            sb.Append("SELECT");
            AppendDbColumns(sb);

            sb.Append($" FROM [Hurace].[{typeof(T).Name}]");

            return sb.ToString();
        }

        public string GenerateGetByIdQuery(int id)
        {
            var sb = new StringBuilder();

            sb.Append("SELECT");
            AppendDbColumns(sb);

            sb.Append($" FROM [Hurace].[{typeof(T).Name}]");
            sb.Append($" WHERE [Id] = {id}");

            return sb.ToString();
        }

        public string GenerateCreateQuery(T entity)
        {
            var sb = new StringBuilder();

            sb.Append($"INSERT INTO [Hurace].[{typeof(T).Name}] (");
            bool firstProperty = true;
            foreach (var currentProperty in typeof(T).GetProperties())
            {
                if (currentProperty.Name != "Id")
                {
                    sb.Append($"{(firstProperty ? "" : ", ")}[{currentProperty.Name}]");
                    firstProperty = false;
                }
            }
            sb.Append($") VALUES (");
            bool firstValue = true;
            foreach (var property in entity.GetType().GetProperties())
            {
                if (property.Name != "Id")
                {
                    var val = entity.GetType().GetProperty(property.Name).GetValue(entity);
                    sb.Append($"{(firstValue ? "" : ", ")}");
                    if (property.PropertyType == typeof(string))
                        sb.Append($"'{val}'");
                    else if (property.PropertyType == typeof(DateTime))
                        sb.Append("'" + ((DateTime)val).ToString("s", DateTimeFormatInfo.InvariantInfo) + "'");
                    else
                        sb.Append($"{val}");
                    firstValue = false;
                }
            }
            sb.Append(")");
            return sb.ToString();
        }

        public string GenerateUpdateQuery(int Id, T entity)
        {
            var sb = new StringBuilder();

            sb.Append($"UPDATE [Hurace].[{typeof(T).Name}] SET");
            bool firstValue = true;
            foreach (var property in entity.GetType().GetProperties())
            {
                if (property.Name != "Id")
                {
                    var val = entity.GetType().GetProperty(property.Name).GetValue(entity);
                    sb.Append($"{(firstValue ? "" : ",")} [{property.Name}] = {val}");
                    firstValue = false;
                }
            }
            sb.Append($" WHERE [Id] = {Id}");
            return sb.ToString();
        }

        public string GenerateDeleteByIdQuery(int Id)
        {
            //TODO: Set Skier Inaktive when there are already entries for him/her else delete Skier entry
            var sb = new StringBuilder();

            sb.Append($"DELETE FROM [Hurace].[{typeof(T).Name}] WHERE Id = {Id}");

            return sb.ToString();
        }

        private void AppendDbColumns(StringBuilder sb)
        {
            bool firstProperty = true;
            foreach (var currentProperty in typeof(T).GetProperties())
            {
                sb.Append($"{(firstProperty ? "" : ",")} [{currentProperty.Name}]");
                firstProperty = false;
            }
        }

    }
}
