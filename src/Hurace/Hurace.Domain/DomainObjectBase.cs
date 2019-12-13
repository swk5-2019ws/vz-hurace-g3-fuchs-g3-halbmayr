using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hurace.Domain
{
    public class DomainObjectBase
    {
        public DomainObjectBase(int id)
        {
            this.Id = id;
            this.PropertiesChanged = false;
        }

        public int Id { get; }
        public bool PropertiesChanged { get; protected set; }

        protected void OnPropertyChanged()
        {
            this.PropertiesChanged = true;
        }
    }
}
