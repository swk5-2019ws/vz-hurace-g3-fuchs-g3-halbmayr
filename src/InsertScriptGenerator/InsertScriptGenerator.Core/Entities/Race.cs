using System;
using System.Collections.Generic;

namespace InsertScriptGenerator.Core
{
    internal class Race
    {
        public int Id { get; set; }
        public int RaceTypeId { get; set; }
        public int VenueId { get; set; }
        public int FirstStartListId { get; set; }
        public int SecondStartListId { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfSensors { get; set; }
        public string Description { get; set; }
        public int GenderSpecificRaceId { get; set; }

        public override string ToString()
        {
            return "INSERT INTO [Hurace].[Race] ([Id], [RaceTypeId], [VenueId], [FirstStartListId], [SecondStartListId], [Date], [NumberOfSensors], [Description], [GenderSpecificRaceId]) "
                + $"VALUES ({Id}, {RaceTypeId}, {VenueId}, {FirstStartListId}, {SecondStartListId}, '{Date.ToString("s")}', {NumberOfSensors}, '{Description}', {GenderSpecificRaceId});";
        }
    }
}
