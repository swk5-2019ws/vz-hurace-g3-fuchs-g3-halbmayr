using System;
using System.Collections.Generic;

namespace InsertScriptGenerator.Core
{
    internal class Season
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public override string ToString()
        {
            return "INSERT INTO [Hurace].[Season] ([Id], [Name], [StartDate], [EndDate]) "
                + $"VALUES ({Id}, '{Name}', '{StartDate.ToString("s")}', '{EndDate.ToString("s")}');";
        }
    }
}
