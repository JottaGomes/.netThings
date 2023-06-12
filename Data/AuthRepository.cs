using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context){
            _context = context;
        }


        public Task<ServiceResponse<string>> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<int>> Resgister(User user, string password){

            var response = new ServiceResponse<int>();

            if (await UserExists(user.Username)){
                response.Succcess = false; 
                response.Message = "User already exists"; 
                return response; 
            }            
            CreatePasswordHash(password, out byte [] passwordHash, out byte [] passwordSalt); 

            user.PasswordHash = passwordHash; 
            user.PasswordSalt = passwordSalt; 

            _context.Users.Add(user); 
            await _context.SaveChangesAsync(); 

            response.Data = user.Id; 

            return response; 
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower())){ // this means user already exists
                return true; 
            }
            return false; 
        }

        public void CreatePasswordHash(string password, out byte [] passwordHash, out byte [] passwordSalt){
            using(var hmac = new System.Security.Cryptography.HMACSHA512()){
                passwordSalt = hmac.Key; 
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); 
            }
        }
    }
}