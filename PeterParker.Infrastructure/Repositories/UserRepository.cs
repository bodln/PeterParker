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
    public class UserRepository : Repository<User>, IUserRepository
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
            IConfiguration config) : base(context)
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
                return "Incorrect Login";
            }
        }

        private async Task<string> BuildTokenAsync(UserDTO request)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, request.Email)
            };

            var user = await userManager.FindByEmailAsync(request.Email);
            var claimList = await context.UserClaims.ToListAsync();

            foreach (var claim in claimList)
            {
                claims.Add(new Claim(claim.ClaimType, claim.ClaimValue));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var expiration = DateTime.UtcNow.AddDays(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                    expires: expiration, signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
