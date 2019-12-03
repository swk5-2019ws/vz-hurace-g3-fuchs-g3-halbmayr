using Hurace.Core.Db.Queries;
using System;
using System.Data.Common;

namespace Hurace.Core.Db.Extensions
{
    public static class DbCommandExtensions
    {
        /// <summary>
        /// Resolve a <see cref="QueryParameter[]"/> and apply it to a <see cref="DbCommand"/>
        /// </summary>
        /// <param name="dbCommand">a command to which the parameters should be applied</param>
        /// <param name="queryParameters">a set of parameters that should be applied</param>
        public static void AddParameters(this DbCommand dbCommand, params QueryParameter[] queryParameters)
        {
            if (dbCommand == null)
            {
                throw new ArgumentNullException(nameof(dbCommand));
            }

            foreach (var queryParam in queryParameters)
            {
                DbParameter dbParam = dbCommand.CreateParameter();

                dbParam.ParameterName = queryParam.ParameterName;
                dbParam.Value = queryParam.Value;

                dbCommand.Parameters.Add(dbParam);
            }
        }
    }
}
