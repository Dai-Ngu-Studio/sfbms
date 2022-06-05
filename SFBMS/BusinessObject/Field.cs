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
        [Column("id")]
        public int id { get; set; }

        [Column("name", TypeName = "nvarchar(255)")]
        public string name { get; set; } = "";

        [Column("description", TypeName = "nvarchar(600)")]
        public string description { get; set; } = "";

        [Column("price", TypeName = "money")]
        public decimal price { get; set; }

        [Column("category_id")]
        public int categoryId { get; set; }
    }
}
