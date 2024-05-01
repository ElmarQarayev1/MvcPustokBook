using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcPustok.Models
{
	public class Book : AuditEntity
    {
        [MaxLength(50)]
        [MinLength(10)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Desc { get; set; }
        [Column(TypeName = "money")]
        public decimal CostPrice { get; set; }
        [Column(TypeName = "money")]
        public decimal SalePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPercent { get; set; }

		public int AuthorId { get; set; }

		public int GenreId {get;set;}

		public bool StockStatus { get; set; }

        public bool IsNew { get; set; }

        public bool IsFeatured { get; set; }

        public Author? Author { get; set; }

		public Genre? Genre { get; set; }

		public List<BookImages>? BookImages { get; set; }
        public List<BookTags>? BookTags { get; set; }
	}
}

