using System;
using System.Threading.Tasks;

namespace Hurace.Domain
{
    public sealed class StartPosition : DomainObjectBase
    {
        public int Position { get; set; }
        public Race Race { get; set; }
        public Skier Skier { get; set; }
    }
}