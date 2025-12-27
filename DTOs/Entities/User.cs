using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_DTOs.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public string? Phone { get; set; }
        public string Address { get; set; }
        public string AvatarUrl { get; set; }
        public string Bio { get; set; }
        public bool IsLocked { get; set; }
        public string KycStatus { get; set; }
        public string IdentityDocumentUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
