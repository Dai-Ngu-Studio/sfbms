using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Category
    {
        public Category()
        {
            Fields = new HashSet<Field>();
            BookingDetails = new HashSet<BookingDetail>();
        }

        [Column("id")]
        public int Id { get; set; }

        [Column("name", TypeName = "nvarchar(255)")]
        public string Name { get; set; } = "";

        public HashSet<Field> Fields { get; set; }

        public ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
