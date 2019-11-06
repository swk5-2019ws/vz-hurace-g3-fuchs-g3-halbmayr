using System.Collections.Generic;

namespace InsertScriptGenerator.Core
{
    internal class RaceType
    {
        public int Id { get; set; }
        public string Label { get; set; }

        public override string ToString()
        {
            return "INSERT INTO [Hurace].[RaceType] ([Id], [Label]) "
                + $"VALUES ({Id}, '{Label}');";
        }
    }
}
