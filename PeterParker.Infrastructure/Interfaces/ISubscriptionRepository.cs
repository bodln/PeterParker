using Microsoft.AspNetCore.Http;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Interfaces
{
    public interface ISubscriptionRepository 
    {
        Task Add(HttpRequest request, SubscriptionDTO subscriptionDTO);
        Task Delete(HttpRequest request);
        Task<SubscriptionDTO> Get(HttpRequest request);
        object Prices();
    }
}
