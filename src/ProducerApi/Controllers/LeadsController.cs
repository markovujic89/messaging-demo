using Lead.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace ProducerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeadsController : ControllerBase
{
    readonly IPublishEndpoint _publishEndpoint;
    
    public LeadsController(IPublishEndpoint publishEndpoint) => _publishEndpoint = publishEndpoint;

    [HttpPost]
    public async Task<IActionResult> CreateLead([FromBody] CreateLeadRequest request)
    {
        var message = new LeadCreated(
            LeadId: Guid.NewGuid(),
            AgencyId: request.AgencyId,
            Name: request.Name,
            Email: request.Email,
            CreatedAt: DateTime.UtcNow
        );

        // publish — MassTransit will map to exchange and routing
        await _publishEndpoint.Publish(message);

        return Accepted(new { message.LeadId });
    }
}