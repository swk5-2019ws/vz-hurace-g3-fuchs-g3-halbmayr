namespace InsertScriptGenerator.Core
{
    internal class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return "INSERT INTO [Hurace].[Country] ([Id], [Name]) "
                + $"VALUES ({Id}, '{Name}');";
        }
    }
}
