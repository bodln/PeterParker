using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PeterParker.Data;
using PeterParker.Data.Models;
using PeterParker.DTOs;
using PeterParker.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly IConfiguration config;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public UserRepository(IMapper mapper, 
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            DataContext context, 
            IConfiguration config) //: base(context)
        {
            this.context = context;
            this.config = config;
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        //Keep in mind the return types
        public async Task<IdentityResult> RegisterUser(UserDTO request)
        {
            var user = mapper.Map<User>(request);

            var result = await userManager.CreateAsync(user, request.Password);

            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "User"));

            return result;
        }

        public async Task<string> LogInUser(UserDTO request)
        {
            var result = await signInManager.PasswordSignInAsync(request.Email,
                    request.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return await BuildTokenAsync(request);
            }
            else
            {
                return "Incorrect Login Information.";
            }
        }

        private async Task<string> BuildTokenAsync(UserDTO request)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.Role, "User"),
            };

            var user = await userManager.FindByEmailAsync(request.Email);
            var claimList = await context.UserClaims.Where(x => x.UserId == user.Id).ToListAsync();

            foreach (var claim in claimList)
            {
                if (claim.ClaimValue != "User")
                {
                    claims.Add(new Claim(claim.ClaimType, claim.ClaimValue));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var expiration = DateTime.UtcNow.AddDays(1);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                issuer: config.GetSection("Jwt:Issuer").Value,
                audience: config.GetSection("Jwt:Audience").Value,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> AddAdminRole(string request)
        {
            var user = await userManager.FindByEmailAsync(request);
            Claim adminClaim = new Claim(ClaimTypes.Role, "Admin");

            if ((await context.UserClaims.Where(x => x.UserId == user.Id).ToListAsync())
                .Where(x => x.ClaimValue == adminClaim.Value).Count() > 0)
            {
                return IdentityResult.Success;
            }

            return await userManager.AddClaimAsync(user, adminClaim);
        }

        public async Task<IdentityResult> RemoveAdminRole(string request)
        {
            var user = await userManager.FindByEmailAsync(request);
            Claim adminClaim = new Claim(ClaimTypes.Role, "Admin");

            if ((await context.UserClaims.Where(x => x.UserId == user.Id).ToListAsync())
                .Where(x => x.ClaimValue == adminClaim.Value).Count() == 0)
            {
                return IdentityResult.Success;
            }

            return await userManager.RemoveClaimAsync(user, adminClaim);
        }

        public async Task<IdentityResult> AddInstructorRole(string request)
        {
            var user = await userManager.FindByEmailAsync(request);
            Claim instsructorClaim = new Claim(ClaimTypes.Role, "Instructor");

            if ((await context.UserClaims.Where(x => x.UserId == user.Id).ToListAsync())
                .Where(x => x.ClaimValue == instsructorClaim.Value).Count() > 0)
            {
                return IdentityResult.Success;
            }

            return await userManager.AddClaimAsync(user, instsructorClaim);
        }

        public async Task<IdentityResult> RemoveInstructorRole(string request)
        {
            var user = await userManager.FindByEmailAsync(request);
            Claim instsructorClaim = new Claim(ClaimTypes.Role, "Instructor");

            if ((await context.UserClaims.Where(x => x.UserId == user.Id).ToListAsync())
                .Where(x => x.ClaimValue == instsructorClaim.Value).Count() == 0)
            {
                return IdentityResult.Success;
            }

            return await userManager.RemoveClaimAsync(user, instsructorClaim);
        }

        public async Task<List<User>> GetAll()
        {
            return await context.Users.ToListAsync();
        }
    }
}
