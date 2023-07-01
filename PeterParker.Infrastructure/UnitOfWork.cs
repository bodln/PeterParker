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
            IGarageRepository garageRepository,
            IInspectorRepository inspectorRepository,
            IParkingSpaceRepository parkingSpaceRepository,
            IPassRepository passRepository,
            ISubscriptionRepository subscriptionRepository,
            ITicketRepository ticketRepository,
            IUserRepository userRepository,
            IZoneRepository zoneRepository
            )
        {
            this.context = context;
            GarageRepository = garageRepository;
            InspectorRepository = inspectorRepository;
            ParkingSpaceRepository = parkingSpaceRepository;
            PassRepository = passRepository;
            SubscriptionRepository = subscriptionRepository;
            TicketRepository = ticketRepository;
            UserRepository = userRepository;
            ZoneRepository = zoneRepository;
        }

        public IGarageRepository GarageRepository { get; }
        public IInspectorRepository InspectorRepository { get; }
        public IParkingSpaceRepository ParkingSpaceRepository { get; }
        public IPassRepository PassRepository { get; }
        public ISubscriptionRepository SubscriptionRepository { get; }
        public ITicketRepository TicketRepository { get; }
        public IUserRepository UserRepository { get; }
        public IZoneRepository ZoneRepository { get; }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

    }
}
