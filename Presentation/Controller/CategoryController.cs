
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using AuctionSystem.Application.Commands.Users;
using AuctionSystem.Application.Commands.Auth;
using AuctionSystem.Application.Queries.Users;
using System.Security.Claims;
using AuctionSystem.Application.DTOs;
using AuctionSystem.Application.Commands.Category;
using AuctionSystem.Application.Queries.Category;
using AuctionSystem.Application.Commands.Categories;

namespace AuctionSystem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { Success = true, Id = id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(list);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteCategoryCommand(id));
            if (!success) return NotFound();
            return Ok(new { Success = true });
        }
    }
}