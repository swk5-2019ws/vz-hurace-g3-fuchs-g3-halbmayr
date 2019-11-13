using Hurace.Core.Db.Utilities;
using System;
using System.Data.Common;

namespace Hurace.Core.Db.Extensions
{
    public static class DbCommandExtensions
    {
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
