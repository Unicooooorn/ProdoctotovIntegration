using MediatR;
using Microsoft.EntityFrameworkCore;
using ProdoctorovIntegration.Application.DbContext;
using ProdoctorovIntegration.Application.Mapping;

namespace ProdoctorovIntegration.Application.Requests.OccupiedDoctorScheduleSlot;

public class GetOccupiedDoctorScheduleSlotRequest : IRequest<IReadOnlyCollection<GetOccupiedDoctorScheduleSlotResponse>>
{ }

public class GetOccupiedDoctorScheduleSlotRequestHandler : IRequestHandler<GetOccupiedDoctorScheduleSlotRequest,
    IReadOnlyCollection<GetOccupiedDoctorScheduleSlotResponse>>
{
    private readonly HospitalDbContext _dbContext;

    public GetOccupiedDoctorScheduleSlotRequestHandler(HospitalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<GetOccupiedDoctorScheduleSlotResponse>> Handle(GetOccupiedDoctorScheduleSlotRequest request, CancellationToken cancellationToken)
    {
        var events = await _dbContext.Event.Where(x => x.IsForProdoctorov).ToListAsync(cancellationToken);

        return events.MapOccupiedSlotsToResponse().ToArray();
    }
}