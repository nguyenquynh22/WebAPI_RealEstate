// File: Common_BLL/Profiles/UserMappingProfile.cs (Đã sửa hoàn chỉnh)

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
            // CHỈ GIỮ LẠI ÁNH XẠ TÙY CHỈNH NÀY (Xóa dòng CreateMap<User, UserResponseDto>();)
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? ""))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone ?? ""))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address ?? ""))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl ?? ""))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio ?? ""));


            // [2] Ánh xạ Create Request DTO -> Entity (User) <-- BỔ SUNG ÁNH XẠ NÀY
            CreateMap<UserCreateRequestDto, User>()
                // Password sẽ được xử lý Hash trong UserService. Ta chỉ map giá trị
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "Customer")) // Role mặc định
                .ForMember(dest => dest.KycStatus, opt => opt.MapFrom(src => "Pending"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());


            // [3] Ánh xạ Update Request DTO -> Entity (Giữ nguyên)
            CreateMap<UserUpdateRequestDto, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.KycStatus, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}