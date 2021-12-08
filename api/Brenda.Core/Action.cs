using System;

namespace Brenda.Core
{
    public class Action : Entity
    {
        protected Action() { }

        public Action(Guid id) : base(id) { }

        public string Name { get; set; }
        public string Image { get; set; }
    }
}
