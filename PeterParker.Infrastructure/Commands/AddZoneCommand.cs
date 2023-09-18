using MediatR;
using PeterParker.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Commands
{
    public class AddZoneCommand : IRequest<ZoneDataDTO>
    {
        public ZoneDTO request { get; set; }
    }
    public class AddZoneCommandHandler : IRequestHandler<AddZoneCommand, ZoneDataDTO>
    {
        private readonly IUnitOfWork unitOfWork;

        public AddZoneCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        async Task<ZoneDataDTO> IRequestHandler<AddZoneCommand, ZoneDataDTO>.Handle(AddZoneCommand request, CancellationToken cancellationToken)
        {
            var response = await unitOfWork.ZoneRepository.Add(request.request);
            return response;
        }
    }
}
