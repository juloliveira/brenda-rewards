using System;

namespace Brenda.Core
{
    public abstract class Entity
    {
        public Entity() : this(Guid.NewGuid()) { }

        public Entity(Guid id) 
        {
            Id = id;
            Tag = Id.ToString().Substring(0, 8).ToUpper();
            CreatedAt = UpdatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; set; }

        public string Tag { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as Entity;

            return item != null && this.Id == item.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
