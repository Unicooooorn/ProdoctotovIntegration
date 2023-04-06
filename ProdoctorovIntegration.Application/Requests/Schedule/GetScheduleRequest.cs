﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProdoctorovIntegration.Application.DbContext;
using ProdoctorovIntegration.Application.Mapping;
using ProdoctorovIntegration.Application.Options;

namespace ProdoctorovIntegration.Application.Requests.Schedule;

public class GetScheduleRequest : IRequest<IReadOnlyCollection<GetScheduleResponse>>
{ }

public class GetScheduleRequestHandler : IRequestHandler<GetScheduleRequest, IReadOnlyCollection<GetScheduleResponse>>
{
    private readonly HospitalDbContext _dbContext;
    private readonly OrganizationNameOptions _organizationNameOptions;

    public GetScheduleRequestHandler(HospitalDbContext dbContext, IOptions<OrganizationNameOptions> options)
    {
        _dbContext = dbContext;
        _organizationNameOptions = options.Value;
    }

    public async Task<IReadOnlyCollection<GetScheduleResponse>> Handle(GetScheduleRequest request, CancellationToken cancellationToken)
    {
        var events = await _dbContext.Event.Where(
            e => e.IsForProdoctorov && e.StartDate > DateTime.UtcNow)
            .ToArrayAsync(cancellationToken);
        return events.MapToResponse(_organizationNameOptions.Name).ToArray();
    }
}