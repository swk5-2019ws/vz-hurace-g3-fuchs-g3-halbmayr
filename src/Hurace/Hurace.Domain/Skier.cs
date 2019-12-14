using System;
using System.Collections.Generic;

namespace Hurace.Domain
{
    public class Skier : DomainObjectBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ImageUrl { get; set; }
        public bool IsRemoved { get; set; }
        public IEnumerable<StartPosition> StartPositions { get; set; }
        public Country Country { get; set; }
        public Sex Sex { get; set; }
    }
}