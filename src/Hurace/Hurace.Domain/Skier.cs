using System;
using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Skier
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CountryId { get; set; }
        public int ImageId { get; set; }
        public int SexId { get; set; }
        public List<int> RaceDataIds { get; }
        public List<int> StartPositionIds { get; }
    }
}
