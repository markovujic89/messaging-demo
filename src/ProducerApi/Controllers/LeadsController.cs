using Lead.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ProducerApi.Services;

namespace ProducerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeadsController : ControllerBase
{
    private readonly ILeadService _leadService;

    public LeadsController(ILeadService leadService)
    {
        _leadService = leadService;
    }


    [HttpPost]
    public async Task<IActionResult> CreateLead([FromBody] CreateLeadRequest request)
    {
        await _leadService.PublishLead(request);

        return Accepted();
    }
}