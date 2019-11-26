using System.Collections.Generic;

namespace InsertScriptGenerator.Core
{
    internal class RaceData
    {
        public int Id { get; set; }
        public int StartListId { get; set; }
        public int SkierId { get; set; }
        public int RaceStateId { get; set; }

        public override string ToString()
        {
            return "INSERT INTO [Hurace].[RaceData] ([Id], [StartListId], [SkierId], [RaceStateId]) "
                + $"VALUES ({Id}, {StartListId}, {SkierId}, {RaceStateId});";
        }
    }
}
