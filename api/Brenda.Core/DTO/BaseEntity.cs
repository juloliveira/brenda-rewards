using System;

namespace Brenda.Core.DTO
{
    public class BaseEntity
    {
        public BaseEntity(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
    }
}
