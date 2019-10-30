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
        public Country Country { get; set; }
        public Image Image { get; set; }
        public Sex Sex { get; set; }
        public List<RaceData> RaceData { get; set; }
    }
}
