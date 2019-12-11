using System;

namespace Hurace.Entities
{
    public class Skier : EntityObjectBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ImageUrl { get; set; }
        public int CountryId { get; set; }
        public int SexId { get; set; }
        public bool IsRemoved { get; set; }
    }
}
