using System;

namespace Sara.Core
{
    public class GenderIdentity : Entity
    {
        protected GenderIdentity() : base() { }

        public GenderIdentity(Guid id) : base(id) { }

        public string Description { get; set; }
    }
}
