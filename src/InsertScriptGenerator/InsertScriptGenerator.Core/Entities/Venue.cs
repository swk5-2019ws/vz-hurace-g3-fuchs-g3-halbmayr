using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace InsertScriptGenerator.Core
{
    internal class Venue
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return "INSERT INTO [Hurace].[Venue] ([Id], [CountryId], [Name]) "
                + $"VALUES ({Id}, {CountryId}, '{Name.Replace("'", "")}');";
        }
    }
}
