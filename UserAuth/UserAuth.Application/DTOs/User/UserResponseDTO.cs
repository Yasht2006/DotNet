using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuth.Application.DTOs.User
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Phone {  get; set; }
        public required string Email { get; set; }
        public required string Role  { get; set; }
        public required string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
