using System;
using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Skier
    {
        public int SkierId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CountryName { get; set; }
        public int ImageId { get; set; }
        public string SexLabel { get; set; }
        public List<int> RaceDataIds { get; set; }
    }
}
