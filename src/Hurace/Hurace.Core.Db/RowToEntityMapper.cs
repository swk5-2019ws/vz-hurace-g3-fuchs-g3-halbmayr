using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Hurace.Core.Db
{
    public class RowToEntityMapper<T> where T : new()
    {
        private IEnumerable<string> SkipableProperties { get; }

        public RowToEntityMapper(IEnumerable<string> skipableProperties)
        {
            SkipableProperties = skipableProperties;
        }

        public T Map(IDataRecord row)
        {
            T entity = new T();

            foreach (var property in entity.GetType().GetProperties())
            {
                if (!SkipableProperties.Any(p => p == property.Name))
                    property.SetValue(entity, Convert.ChangeType(row[property.Name], property.PropertyType));
            }

            return entity;
        }
    }
}
