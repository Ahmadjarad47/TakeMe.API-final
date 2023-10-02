using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMe.Core.Entities;
using TakeMe.DTOs;
using TakeMe.Core.DTOs;
using System.Security.Claims;

namespace TakeMe.Core.Interfaces
{
    public interface ITokenService
    {
        string GetAndCreateToken(AppUsers token);
        ClaimsPrincipal GetPrincipalFromRefreshToken(string accessToken);
        string CreateRefreshToken();
        void DeleteUser(string id);
         Task AddUserAsync(AppUsers appUsers);
    }
}
