using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PeterParker.Data;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.DTOs;
using PeterParker.Infrastructure.Exceptions;
using PeterParker.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly IConfiguration config;
        private readonly ILogger<User> logger;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public UserRepository(IMapper mapper, 
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            DataContext context, 
            IConfiguration config,
            ILogger<User> logger) //: base(context)
        {
            this.context = context;
            this.config = config;
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        //Keep in mind the return types
        public async Task RegisterUser(UserRegisterDTO request)
        {
            if (await userManager.FindByEmailAsync(request.Email) != null)
            {
                throw new EmailTakenException($"Account with the email address: {request.Email}, already exists.");
            }

            var user = mapper.Map<User>(request);

            logger.LogInformation("Adding user.");

            await userManager.CreateAsync(user, request.Password);

            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "User"));

            logger.LogInformation("User added.");
        }

        public async Task<UserDataDTO> ReturnUserData(HttpRequest request)
        {
            logger.LogInformation("Getting user.");

            string token = request.Headers["Authorization"].ToString().Replace("bearer ", "");

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            string email = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;

            var user = await userManager.FindByEmailAsync(email);
            var userDTO = mapper.Map<UserDataDTO>(user);

            logger.LogInformation("Success.");

            return userDTO;

        }

        public async Task<AuthTokens> LogInUser(UserLoginDTO request)
        {
            logger.LogInformation("Signing user in.");

            var result = await signInManager.PasswordSignInAsync(request.Email,
                    request.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                string token = await BuildTokenAsync(request.Email);

                var refreshToken = GenerateRefershToken();
                await SetRefreshToken(refreshToken, request.Email);

                logger.LogInformation("User signed in.");

                return new AuthTokens
                {
                    Token = token,
                    RefreshToken = refreshToken.Token
                };
            }
            else
            {
                throw new IncorrectLoginInfoException();
            }
        }

        private RefreshToken GenerateRefershToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private async Task SetRefreshToken(RefreshToken newRefreshToken, string email)
        {
            //User user = await userManager.FindByEmailAsync(email);
            User user = context.Users
                .Include(u => u.RefreshToken)
                .Where(u => u.Email == email)
                .FirstOrDefault();

            if (user.RefreshToken != null)
            {
                context.RefreshTokens.Remove(user.RefreshToken);
            }
            
            //context.RefreshTokens.Remove()
            context.RefreshTokens.Add(newRefreshToken);
            await context.SaveChangesAsync();

            user.RefreshToken = newRefreshToken; 
            await userManager.UpdateAsync(user);
        }

        private async Task<string> BuildTokenAsync(string userEmail)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, userEmail),
                new Claim(ClaimTypes.Role, "User"),
            };

            var user = await userManager.FindByEmailAsync(userEmail);
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

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddSeconds(30),
                issuer: config.GetSection("Jwt:Issuer").Value,
                audience: config.GetSection("Jwt:Audience").Value,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthTokens> TokenRefresh(string refreshToken)
        {

            if (refreshToken == null)
            {
                throw new InvalidRefreshToken();
            }

            var userRefrehToken = context.RefreshTokens
                .Where(rt => rt.Token == refreshToken)
                .FirstOrDefault();

            if (userRefrehToken == null)
            {
                throw new InvalidRefreshToken();
            }

            if (userRefrehToken.Expires < DateTime.Now)
            {
                throw new InvalidRefreshToken();
            }

            User user = await context.Users
                .Include(u => u.RefreshToken)
                .Where(u => u.RefreshToken.Token.Equals(refreshToken))
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
            // Make a seperate create token function or put it in User class
            string token = await BuildTokenAsync(user.Email);

            var newRefreshToken = GenerateRefershToken();
            await SetRefreshToken(newRefreshToken, user.Email);

            return new AuthTokens
            {
                Token = token,
                RefreshToken = newRefreshToken.Token
            };
        }

        public async Task AddAdminRole(UserLoginDTO request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            Claim adminClaim = new Claim(ClaimTypes.Role, "Admin");

            if ((await context.UserClaims.Where(x => x.UserId == user.Id).ToListAsync())
                .Where(x => x.ClaimValue == adminClaim.Value).Count() > 0)
            {
                //can also throw exception
                return;
            }

            await userManager.AddClaimAsync(user, adminClaim);
        }

        public async Task RemoveAdminRole(UserLoginDTO request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            Claim adminClaim = new Claim(ClaimTypes.Role, "Admin");

            if ((await context.UserClaims.Where(x => x.UserId == user.Id).ToListAsync())
                .Where(x => x.ClaimValue == adminClaim.Value).Count() == 0)
            {
                return;
            }
            await userManager.RemoveClaimAsync(user, adminClaim);
        }

        public async Task AddInspectorRole(UserLoginDTO request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            Claim instsructorClaim = new Claim(ClaimTypes.Role, "Inspector");

            if ((await context.UserClaims.Where(x => x.UserId == user.Id).ToListAsync())
                .Where(x => x.ClaimValue == instsructorClaim.Value).Count() > 0)
            {
                return;
            }
            
            await userManager.AddClaimAsync(user, instsructorClaim);
        }

        public async Task RemoveInspectorRole(UserLoginDTO request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            Claim instsructorClaim = new Claim(ClaimTypes.Role, "Inspector");

            if ((await context.UserClaims.Where(x => x.UserId == user.Id).ToListAsync())
                .Where(x => x.ClaimValue == instsructorClaim.Value).Count() == 0)
            {
                return;
            }
            
            await userManager.RemoveClaimAsync(user, instsructorClaim);
        }

        public async Task<List<UserDataDTO>> GetAll()
        {
            logger.LogInformation("Getting users.");

            var users = await context.Users
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ThenBy(u => u.Email)
                .ToListAsync();

            List<UserDataDTO> usersDTO = mapper.Map<List<UserDataDTO>>(users);

            foreach (var userDTO in usersDTO)
            {
                userDTO.Vehicles = mapper
                    .Map<List<VehicleDTO>>(await context.Vehicles
                    .Where(v => v.User.Email == userDTO.Email)
                    .OrderBy(v => v.Registration)
                    .ToListAsync());
            }

            return usersDTO;
        }
    }
}
