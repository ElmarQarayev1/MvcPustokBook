using System;
namespace MvcPustok.Models
{
	public class AuditEntity:BaseEntity
	{
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

