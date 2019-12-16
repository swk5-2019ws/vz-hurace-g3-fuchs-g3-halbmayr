using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hurace.Domain
{
    public class Associated<T> where T : DomainObjectBase
    {
        private int? foreignKey;
        private T reference;

        public Associated() { }

        public Associated(int foreignKey)
        {
            this.foreignKey = foreignKey;
        }

        public Associated(T reference)
        {
            this.reference = reference ?? throw new ArgumentNullException(nameof(reference));
            this.foreignKey = null;
        }

        public int? ForeignKey
        {
            get => foreignKey;
            set
            {
                if (reference == null)
                    this.foreignKey = value;
                else
                    throw new InvalidOperationException(
                        $"reference already set, setting the foreign-key makes no sense for {typeof(T).FullName}");
            }
        }

        public T Reference
        {
            get => this.reference;
            set
            {
                if (this.foreignKey.HasValue && value != null)
                    throw new InvalidOperationException(
                        $"foreign-key already set, setting the reference makes no sense for {typeof(T).FullName}");
                this.reference = value;
            }
        }

        [JsonIgnore]
        public bool HasReference => !this.foreignKey.HasValue;
    }
}
