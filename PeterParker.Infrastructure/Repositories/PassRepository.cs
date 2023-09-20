using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeterParker.Data;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.Infrastructure.Exceptions;
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
    public class PassRepository : IPassRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly ILogger<PassRepository> logger;

        public PassRepository(DataContext context,
            IMapper mapper,
            ILogger<PassRepository> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<PassDTO> Add(HttpRequest request, PassCreationDTO passCreationDTO)
        {
            logger.LogInformation("Adding pass.");

            User user = await GetUser(request);

            List<Zone> zones = new List<Zone>();

            foreach (Guid guid in passCreationDTO.ZoneGUIDs)
            {
                Zone zone = await context.Zones.Where(z => z.GUID == guid).FirstOrDefaultAsync();
                if (zone == null)
                {
                    throw new NotFoundException($"Zone with the GUID {guid}, does not exist.");
                }
                zones.Add(zone);
            }

            float finalPrice = 0;
            foreach (Zone zone in zones)
            {
                finalPrice += zone.HourlyRate * passCreationDTO.Hours;
            }

            if (zones.Count() > 6)
            {
                finalPrice = (float)(finalPrice * 0.4);
            }
            else
            {
                finalPrice = (float)(finalPrice * (1 - (0.1 * (zones.Count() - 1))));
            }

            Pass pass = new Pass
            {
                GUID = Guid.NewGuid(),
                TimeOfSale = DateTime.Now,
                Expiration = DateTime.Now.AddHours(passCreationDTO.Hours),
                Price = finalPrice,
                Zones = zones
            };

            context.Passes.Add(pass);
            user.Pass = pass;

            context.SaveChanges();

            return mapper.Map<PassDTO>(pass);
        }

        private async Task<User> GetUser(HttpRequest request)
        {
            logger.LogInformation("Getting user.");

            string token = request.Headers["Authorization"].ToString().Replace("bearer ", "");

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            string email = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;

            User user = await context.Users
                .Include(u => u.Pass)
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            logger.LogInformation("Success.");

            return user;
        }
    }
}
