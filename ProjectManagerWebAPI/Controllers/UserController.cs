using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagerWebAPI.Dtos.User;
using ProjectManagerWebAPI.Models;
using ProjectManagerWebAPI.Service;

namespace ProjectManagerWebAPI.Controllers
{
    [Route("/api/Account/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;

        public UserController(UserManager<User> userManager, IMapper mapper, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var user = _mapper.Map<User>(userDto);
                //user.PasswordHash = _userManager.PasswordHasher.HashPassword(user,userDto.Password);
                var createdUser = await _userManager.CreateAsync(user, userDto.Password);
                if (createdUser.Succeeded)
                {

                    var role = await _userManager.AddToRoleAsync(user, "User");
                    if (role.Succeeded)
                    {
                        return Ok(new
                        {
                            UserName = user.UserName,
                            Email = user.Email,
                            Token = _tokenService.CreateToken(user)
                        });
                    }
                    else
                    {
                        return StatusCode(500, role.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginDto.UserName.ToLower());
           
            if (user == null) { return Unauthorized("Invalid userName"); }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) { return Unauthorized("Invalid userName and/or password"); }
            return Ok(new
            {
                UserName = user.UserName,
                Role = await _userManager.IsInRoleAsync(user,"Admin")? "Admin":"User",
                Token = _tokenService.CreateToken(user)
            });

        }
      
    }
}

