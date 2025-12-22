// File: Common.DTOs/DTOs/UserUpdateRequestDto.cs

using System.ComponentModel.DataAnnotations;

namespace Common_DTOs.DTOs
{
    public class UserUpdateRequestDto
    {
        // Các trường này đều là nullable (string?) vì Client có thể chỉ gửi một phần

        [StringLength(50)]
        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? Address { get; set; }
        public string? Role { get; set; }

        public string? AvatarUrl { get; set; }

        public string? Bio { get; set; }

        public bool? IsLocked { get; set; }
    }
}