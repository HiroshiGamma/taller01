using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller01.src.models;

namespace taller01.src.Interface
{
    public interface ITokenService
    {
       string CreateToken(AppUser user);
    }
}