using System.Collections.Generic;

namespace InsertScriptGenerator.Core
{
    internal class RaceState
    {
        public int Id { get; set; }
        public string Label { get; set; }

        public override string ToString()
        {
            return "INSERT INTO [Hurace].[RaceState] ([Id], [Label]) "
                + $"VALUES ({Id}, '{Label}');";
        }
    }
}
