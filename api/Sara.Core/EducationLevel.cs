using System;

namespace Sara.Core
{

    public class EducationLevel : Entity
    {
        protected EducationLevel() : base() { }
        public EducationLevel(Guid id) : base(id) { }
        public string Description { get; set; }
    }
}
