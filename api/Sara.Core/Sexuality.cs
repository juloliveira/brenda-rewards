using System;

namespace Sara.Core
{
    public class Sexuality : Entity
    {
        protected Sexuality() { }

        public Sexuality(Guid id) : base(id) { }

        public string Description { get; set; }
    }
}
