using System;

namespace ConfigManager.Entities.DbClass
{
    public interface IEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
    }
}