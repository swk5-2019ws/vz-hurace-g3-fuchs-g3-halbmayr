using System.Collections.Generic;

namespace Hurace.Domain.Entities
{
    public class Sex
    {
        public string Label { get; set; }
        public List<Skier> Skiers { get; set; }
    }
}
