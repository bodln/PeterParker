using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeterParker.Data;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.Infrastructure.Exceptions;
using PeterParker.Infrastructure.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PeterParker.Infrastructure.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly ILogger<Subscription> logger;
        private readonly UserManager<Subscription> userManager;

        public SubscriptionRepository(DataContext context,
            IMapper mapper,
            ILogger<Subscription> logger,
            UserManager<Subscription> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
            this.userManager = userManager;
        }

        public async Task Add(HttpRequest request)
        {
            User user = await GetUser(request);

            logger.LogInformation("Creating subscription.");

            Subscription subscription = new Subscription
            {
                GUID = Guid.NewGuid(),
                Expiration = DateTime.Now.AddMonths(1)
            };

            user.Subscription = subscription;

            context.Subscriptions.Add(subscription);
            context.SaveChanges();

            logger.LogInformation("Success.");
        }

        public async Task<SubscriptionDTO> Get(HttpRequest request)
        {
            User user = await GetUser(request);

            if (user.Subscription == null)
            {
                logger.LogWarning("User has no subscription.");
            }

            SubscriptionDTO subscriptionDTO = mapper.Map<SubscriptionDTO>(user.Subscription);

            return subscriptionDTO;
        }

        public async Task Delete(SubscriptionDTO subscriptionDTO)
        {
            logger.LogInformation("Deleting subscription.");

            Subscription subscription = await context.Subscriptions
                .Where(s => s.GUID == subscriptionDTO.GUID)
                .FirstOrDefaultAsync();

            if (subscription == null)
            {
                throw new NotFoundException("Subscription not found.");
            }

            context.Subscriptions.Remove(subscription);
            context.SaveChanges();

            logger.LogInformation("Success.");
        }

        private async Task<User> GetUser(HttpRequest request)
        {
            logger.LogInformation("Deleting subscription.");

            string token = request.Headers["Authorization"].ToString().Replace("bearer ", "");

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            string email = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;

            User user = await context.Users
                .Include(u => u.Subscription)
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
