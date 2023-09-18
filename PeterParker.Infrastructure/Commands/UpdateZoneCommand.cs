using MediatR;
using PeterParker.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Commands
{
    public class UpdateZoneCommand : IRequest<ZoneDataDTO>
    {
        public ZoneDTO request { get; set; }
    }
    public class UpdateZoneCommandHandler : IRequestHandler<UpdateZoneCommand, ZoneDataDTO>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateZoneCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        async Task<ZoneDataDTO> IRequestHandler<UpdateZoneCommand, ZoneDataDTO>.Handle(UpdateZoneCommand request, CancellationToken cancellationToken)
        {
            var response = await unitOfWork.ZoneRepository.Update(request.request);
            return response;
        }
    }
}
