using System.Collections.Generic;

namespace InsertScriptGenerator.Core
{
    internal class Sex
    {
        public int Id { get; set; }
        public string Label { get; set; }

        public override string ToString()
        {
            return "INSERT INTO [Hurace].[Sex] ([Id], [Label]) "
                + $"VALUES ({Id}, '{Label}');";
        }
    }
}
