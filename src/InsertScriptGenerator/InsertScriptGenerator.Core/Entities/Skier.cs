using System;
using System.Collections.Generic;

namespace InsertScriptGenerator.Core
{
    internal class Skier
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ImageUrl { get; set; }
        public int CountryId { get; set; }
        public int SexId { get; set; }

        public override string ToString()
        {
            return "INSERT INTO [Hurace].[Skier] ([Id], [FirstName], [LastName], [DateOfBirth], [CountryId], [ImageUrl], [SexId]) "
                + $"VALUES ({Id}, '{FirstName}', '{LastName}', '{DateOfBirth.ToString("s")}', {CountryId}, {ImageUrl}, {SexId});";
        }
    }
}
