using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Resgister (User user, string password);
        Task<ServiceResponse<string>> Login (string username, string password); 
        Task<bool> UserExists (string username); 
    }
}