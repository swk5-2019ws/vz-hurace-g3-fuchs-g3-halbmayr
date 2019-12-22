using System;
using System.Collections.Generic;

namespace Hurace.Domain
{
    public sealed class Skier : DomainObjectBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ImageUrl { get; set; }
        public bool IsRemoved { get; set; }
        public IEnumerable<Associated<StartPosition>> StartPositions { get; set; }
        public Associated<Country> Country { get; set; }
        public Associated<Sex> Sex { get; set; }
    }
}