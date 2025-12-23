using Common_DTOs.DTOs;
using Common_DTOs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_BLL.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(UserResponseDto user);
    }
}
