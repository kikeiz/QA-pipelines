using Microsoft.AspNetCore.Mvc;
using QA.Application.Ports.Inbound;
using QA.Domain.Entities;
using QA.Application.DTOs;

namespace QA.ClientInterface.Controllers
{
    [ApiController]
    [Route("processes")]
    public class QAProcessController(
        ILogger<QAProcessController> logger,
        IServiceProvider serviceProvider
        ) : ControllerBase
    {

        [HttpGet("{processId}")]
        public async Task<IActionResult> GetById(Guid processId)
        {
            logger.LogInformation("Fetching QA Process with ID: {ProcessId}", processId);

            // Create a new scope to resolve scoped services
            using IServiceScope scope = serviceProvider.CreateScope();
            var findQAprocessUseCase = scope.ServiceProvider.GetRequiredService<IQAProcessFinder>();
            QAProcess qaProcess = await findQAprocessUseCase.Find(processId);

            return qaProcess is null
                ? NotFound(new { status = "error", message = "QA Process not found" })
                : Ok(new { status = "success", data = new { process = qaProcess } });
        }

        [HttpGet()]
        public async Task<IActionResult> FilterByUserId([FromHeader(Name = "x-user-id")] Guid userId)
        {

            logger.LogInformation("Fetching QA Processes: {UserId}", userId);

            using IServiceScope scope = serviceProvider.CreateScope();
            var getProcessUserCase = scope.ServiceProvider.GetRequiredService<IQAGetProcesses>();
            GetQAProcessesResultDto qaProcesses = await getProcessUserCase.GetProcesses(
                userId
            );

            return qaProcesses is null
                ? NotFound(new { status = "error", message = "QA Process not found" })
                : Ok(new { status = "success", data = qaProcesses });
        }
    }
}
