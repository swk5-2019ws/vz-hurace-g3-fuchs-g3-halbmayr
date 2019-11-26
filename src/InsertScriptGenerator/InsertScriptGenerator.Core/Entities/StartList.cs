using System.Collections.Generic;

namespace InsertScriptGenerator.Core
{
    internal class StartList
    {
        public int Id { get; set; }

        public override string ToString()
        {
            return "INSERT INTO [Hurace].[StartList] ([Id]) "
                + $"VALUES ({Id});";
        }
    }
}
