namespace InsertScriptGenerator.Core
{
    internal class StartPosition
    {
        public int StartListId { get; set; }
        public int SkierId { get; set; }
        public int Position { get; set; }

        public override string ToString()
        {
            return "INSERT INTO [Hurace].[StartPosition] ([StartListId], [SkierId], [Position]) "
                + $"VALUES ({StartListId}, {SkierId}, '{Position}');";
        }
    }
}
