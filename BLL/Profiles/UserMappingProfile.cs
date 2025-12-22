// File: Common_BLL/Profiles/UserMappingProfile.cs (Đã sửa)

using AutoMapper;
using Common_DTOs.DTOs;
using Common_DTOs.Entities;

namespace Common_BLL.Profiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // [1] Ánh xạ Entity (User) -> Response DTO (UserResponseDto)
            // Khắc phục lỗi trùng lặp và loại bỏ các trường bị xóa khỏi DTO
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? ""))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone ?? ""))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address ?? ""))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl ?? ""))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio ?? ""));


            // [2] Ánh xạ Create Request DTO -> Entity (User) <-- BỔ SUNG ÁNH XẠ NÀY
            CreateMap<UserCreateRequestDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)) // Tạm thời map, sau đó UserService sẽ hash
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "Customer")) // Hoặc bất kỳ role mặc định nào
                .ForMember(dest => dest.KycStatus, opt => opt.MapFrom(src => "Pending"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());


            // [3] Ánh xạ Update Request DTO -> Entity (Giữ nguyên)
            CreateMap<UserUpdateRequestDto, User>()
                // ... (Các ForMember giữ nguyên)
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}