using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    [Table("Field")]
    public class Field
    {
        public Field()
        {
            Fields = new HashSet<Field>();
        }

        [Column("id")]
        public int Id { get; set; }

        [Column("name", TypeName = "nvarchar(255)")]
        public string Name { get; set; } = "";

        [Column("description", TypeName = "nvarchar(600)")]
        public string Description { get; set; } = "";

        [Column("price", TypeName = "money")]
        public decimal Price { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public ICollection<Field> Fields { get; set; }
    }
}
