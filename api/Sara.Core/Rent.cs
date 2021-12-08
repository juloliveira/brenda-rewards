using System;

namespace Sara.Core
{
    public class Income : Entity
    {
        protected Income() : base() { }

        public Income(Guid id) : base(id) { }

        public string Description { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
