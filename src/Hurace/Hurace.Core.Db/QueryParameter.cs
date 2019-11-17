#pragma warning disable IDE0045 // Convert to conditional expression
namespace Hurace.Core.Db
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
            else
                this.Value = value;
        }
    }
}
