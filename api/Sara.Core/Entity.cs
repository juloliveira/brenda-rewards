using System;

namespace Sara.Core
{
    public abstract class Entity
    {
        public Entity() : this(Guid.NewGuid()) { }
        public Entity(Guid id)
        {
            Id = id;
            Tag = Id.ToString().Substring(0, 8);
            CreatedAt = UpdatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; set; }

        public string Tag { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
