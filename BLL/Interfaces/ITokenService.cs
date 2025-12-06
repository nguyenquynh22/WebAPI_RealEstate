using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_DTOs.Entities;

namespace Common_BLL.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
