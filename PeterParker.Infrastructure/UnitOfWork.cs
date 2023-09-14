using PeterParker.Data;
using PeterParker.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext context;

        public UnitOfWork(
            DataContext context,
            IParkingSpaceRepository parkingSpaceRepository,
            IPassRepository passRepository,
            ISubscriptionRepository subscriptionRepository,
            ITicketRepository ticketRepository,
            IUserRepository userRepository,
            IZoneRepository zoneRepository,
            IVehicleRepository vehicleRepository
            )
        {
            this.context = context;
            ParkingSpaceRepository = parkingSpaceRepository;
            PassRepository = passRepository;
            SubscriptionRepository = subscriptionRepository;
            TicketRepository = ticketRepository;
            UserRepository = userRepository;
            ZoneRepository = zoneRepository;
            VehicleRepository = vehicleRepository;
        }

        public IParkingSpaceRepository ParkingSpaceRepository { get; }
        public IPassRepository PassRepository { get; }
        public ISubscriptionRepository SubscriptionRepository { get; }
        public ITicketRepository TicketRepository { get; }
        public IUserRepository UserRepository { get; }
        public IZoneRepository ZoneRepository { get; }
        public IVehicleRepository VehicleRepository { get; }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

    }
}
