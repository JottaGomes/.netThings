using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dtos.Character.User;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Resgister (UserRegisterDto request){

            var response = await _authRepository.Resgister(
                new User {Username = request.Username}, request.Password
            ); 
            if (!response.Succcess){
                return BadRequest(response); 
            }
            return Ok(response); 
        }
        
        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login (UserLoginDto request){

            var response = await _authRepository.Login(request.Username, request.Password); 
            if (!response.Succcess){
                return BadRequest(response); 
            }
            return Ok(response); 
        }
    }
} 