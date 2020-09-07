using ASBlog.API.Models.Auth;
using ASBlog.API.Resources;
using ASBlog.API.Settings;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        private readonly JwtSettings _jwtSettings;

        public AuthController(
            IMapper mapper,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOptionsSnapshot<JwtSettings> jwtSettings)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterResource registerResource)
        {
            var user = _mapper.Map<RegisterResource, User>(registerResource);

            var userCreateResult = await _userManager.CreateAsync(user, registerResource.Password);

            if (userCreateResult.Succeeded)
            {

                await _userManager.AddToRoleAsync(user, "Writer");

                return Created(string.Empty, string.Empty);
            }

            return Problem(userCreateResult.Errors.First().Description, null, 500);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginResource loginResource)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == loginResource.Email);
            if (user is null)
            {
                return NotFound("User not found");
            }

            var userSigninResult = await _userManager.CheckPasswordAsync(user, loginResource.Password);

            if (userSigninResult)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(GenerateJwt(user, roles));
            }

            return BadRequest("Email or password incorrect.");
        }

        [HttpPost("Roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name should be provided.");
            }

            var newRole = new Role
            {
                Name = roleName
            };

            var roleResult = await _roleManager.CreateAsync(newRole);

            if (roleResult.Succeeded)
            {
                return Ok();
            }

            return Problem(roleResult.Errors.First().Description, null, 500);
        }

        
        [HttpPost("User/Role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserToRole(AddUserToRoleResource addUserToRoleResource)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == addUserToRoleResource.UserEmail);

            var result = await _userManager.AddToRoleAsync(user, addUserToRoleResource.RoleName);

            if (result.Succeeded)
            {
                return Ok();
            }

            return Problem(result.Errors.First().Description, null, 500);
        }

        [HttpGet("User")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == username);

            var userRs = _mapper.Map<User, UserResource>(user);

            var userRoles = await _userManager.GetRolesAsync(user);

            userRs.Roles = userRoles.ToList();


            return Ok(userRs);
        }

        [HttpGet("UserByEmail/{Email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserByEmail(string Email)
        {
 
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == Email);

            if(user == null)
            {
                return NotFound();
            }

            var userRs = _mapper.Map<User, UserResource>(user);

            var userRoles = await _userManager.GetRolesAsync(user);

            userRs.Roles = userRoles.ToList();


            return Ok(userRs);
        }

        [HttpGet("AllUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            List<UserResource> usersRs = new List<UserResource>();

            var users = await _userManager.Users.ToListAsync();
            
            foreach(var item in users)
            {
                var user = _mapper.Map<User, UserResource>(item);

                var userRoles = await _userManager.GetRolesAsync(item);

                user.Roles = userRoles.ToList();

                usersRs.Add(user);
            }

            return Ok(usersRs);

        }

        private string GenerateJwt(User user, IList<string> roles)
        {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
            claims.AddRange(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtSettings.ExpirationInDays));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
