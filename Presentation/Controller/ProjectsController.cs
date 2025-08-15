using AuctionSystem.Domain.Constants;
using Microsoft.AspNetCore.Mvc;
using AuctionSystem.Application.Commands.Projects;
using AuctionSystem.Application.Queries.Projects;
using MediatR;

namespace AuctionSystem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllProjectsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _mediator.Send(new GetProjectByIdQuery(id));
            if (dto == null) return NotFound("Project not found");
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectCommand cmd)
        {
            var newId = await _mediator.Send(cmd);
            return Ok(new { Success = true, ProjectId = newId });
        }
        [HttpGet("state/{state}")]
        public async Task<IActionResult> GetByState(ProjectStatus state)
        {
            var dtos = await _mediator.Send(new GetProjectByStateQuery(state));
            return Ok(dtos);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var result = await _mediator.Send(new DeleteProjectCommand(id));
            return result ? Ok(new { Success = true, Message = "Project deleted" })
                        : NotFound(new { Success = false, Message = "Project not found" });
        }
        [HttpPost("{id}/submit")]
        public async Task<IActionResult> SubmitProject(int id)
        {
            var result = await _mediator.Send(new SubmitProjectCommand(id));
            if (!result.Success) return BadRequest(new { result.Success, result.ErrorMessage });
            return Ok(result);
        }

    }
}
