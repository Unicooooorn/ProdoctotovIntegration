using MediatR;
using Microsoft.EntityFrameworkCore;
using ProdoctorovIntegration.Application.DbContext;
using ProdoctorovIntegration.Application.Mapping;

namespace ProdoctorovIntegration.Application.Requests.Schedule;

public class GetScheduleRequest : IRequest<IReadOnlyCollection<GetScheduleResponse>>
{ }

public class GetScheduleRequestHandler : IRequestHandler<GetScheduleRequest, IReadOnlyCollection<GetScheduleResponse>>
{
    private readonly HospitalDbContext _dbContext;

    public GetScheduleRequestHandler(HospitalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<GetScheduleResponse>> Handle(GetScheduleRequest request, CancellationToken cancellationToken)
    {
        var events = await _dbContext.Event.Where(
            e => e.IsForProdoctorov)
            .ToArrayAsync(cancellationToken);
        return events.MapToResponse().ToArray();
    }
}