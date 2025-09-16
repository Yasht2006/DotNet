using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Domain.Common;

namespace UserAuth.Domain.Models
{
    public class User : BaseEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public required string Name { get; set; }
        [Column("phone")]
        public required string Phone { get; set; }
        [Column("email")]
        public required string Email { get; set; }
        [Column("password")]
        public required string Password { get; set; }
        [Column("role")]
        public required string Role { get; set; }

        public User? CreatedByUser { get; set; }
        public User? UpdatedByUser { get; set; }
    }
}
