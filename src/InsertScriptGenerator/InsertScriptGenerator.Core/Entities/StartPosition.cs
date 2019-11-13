namespace InsertScriptGenerator.Core
{
    internal class StartPosition
    {
        public int Id { get; set; }
        public int StartListId { get; set; }
        public int SkierId { get; set; }
        public int Position { get; set; }

        public override string ToString()
        {
            return "INSERT INTO [Hurace].[StartPosition] ([Id], [StartListId], [SkierId], [Position]) "
                + $"VALUES ({Id}, {StartListId}, {SkierId}, '{Position}');";
        }
    }
}
