using System;
using System.Data;

namespace Hurace.Core.Db.Utilities
{
    public sealed class RowMapper<T> where T : new()
    {
        /// <summary>
        /// Generic Map implementation that takes a single result-row and maps the single columns
        /// with reflection to type <see cref="T"/>
        /// </summary>
        /// <typeparam name="T">defines to which type the result-row should be mapped</typeparam>
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
