using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Hurace.Core.Db.Utilities
{
    public class RowMapper<T> where T : new()
    {
        public T Map(IDataRecord row)
        {
            if (row is null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            T entity = new T();

            foreach (var property in entity.GetType().GetProperties())
            {
                property.SetValue(entity, Convert.ChangeType(row[property.Name], property.PropertyType));
            }

            return entity;
        }
    }
}
