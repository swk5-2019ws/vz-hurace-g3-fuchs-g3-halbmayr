namespace Hurace.Core.Db.Utilities
{
    public class QueryParameter
    {
        public string ParameterName { get; set; }
        public object Value { get; set; }

        internal QueryParameter(string name, object value)
        {
            this.ParameterName = name;
            this.Value = value;
        }
    }
}
