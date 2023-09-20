using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        private float weeklyPrice = 10;
        private float mounthlyPrice = 30;
        private float annualPrice = 250;

        public SubscriptionRepository(DataContext context,
            IMapper mapper,
            ILogger<Subscription> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public object Prices()
        {
            return new
            {
                weekly = weeklyPrice,
                mounthlyPrice = mounthlyPrice,
                annualPrice = annualPrice
            };
        }

        public async Task Add(HttpRequest request, SubscriptionDTO subscriptionDTO)
        {
            User user = await GetUser(request);

            logger.LogInformation("Creating subscription.");

            Subscription subscription = new Subscription
            {
                GUID = Guid.NewGuid()
            };

            switch (subscriptionDTO.Type)
            {
                case "weekly":
                    subscription.Expiration = DateTime.Now.AddDays(7);
                    subscription.Price = weeklyPrice;
                    break;
                case "mounthly":
                    subscription.Expiration = DateTime.Now.AddMonths(1);
                    subscription.Price = mounthlyPrice;
                    break;
                case "annually":
                    subscription.Expiration = DateTime.Now.AddYears(1);
                    subscription.Price = annualPrice;
                    break;
                default:
                    throw new NotFoundException($"No subscription of type {subscriptionDTO.Type}.");
            }


            context.Subscriptions.Add(subscription);
            user.Subscription = subscription;
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
