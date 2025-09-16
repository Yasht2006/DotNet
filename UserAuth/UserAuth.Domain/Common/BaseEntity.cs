using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuth.Domain.Common
{
    public class BaseEntity
    {
        [Column("created_by")]
        public int? CreatedBy { get; set; }
        [Column("created_on")]
        public DateTime? CreatedOn { get; set; }
        [Column("updated_by")]
        public int? UpdatedBy { get;set; }
        [Column("updated_on")]
        public DateTime? UpdatedOn { get; set; }
    }
}
