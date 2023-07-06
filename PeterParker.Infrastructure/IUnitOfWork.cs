using PeterParker.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure
{
    public interface IUnitOfWork
    {
        public IGarageRepository GarageRepository { get; }
        public IParkingSpaceRepository ParkingSpaceRepository { get; }
        public IPassRepository PassRepository { get; }
        public ISubscriptionRepository SubscriptionRepository { get; }
        public ITicketRepository TicketRepository { get; }
        public IUserRepository UserRepository { get; }
        public IZoneRepository ZoneRepository { get; }
        public IVehicleRepository VehicleRepository { get; }

        void SaveChanges();
    }
}
