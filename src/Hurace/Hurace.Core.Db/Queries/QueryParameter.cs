#pragma warning disable IDE0045 // Convert to conditional expression
using System;

namespace Hurace.Core.Db.Queries
{
    public class QueryParameter
    {
        public string ParameterName { get; set; }
        public object Value { get; set; }

        internal QueryParameter(string name, object value)
        {
            this.ParameterName = name;

            if (value.GetType() == typeof(bool))
                this.Value = (bool)value ? "TRUE" : "FALSE";
            else if (value is DateTime valueAsDateTime)
                this.Value = valueAsDateTime.ToString("s");
            else
                this.Value = value;
        }
    }
}
