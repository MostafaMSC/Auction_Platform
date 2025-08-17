using AuctionSystem.Domain.Constants;
using Microsoft.AspNetCore.Mvc;
using AuctionSystem.Application.Commands.Projects;
using AuctionSystem.Application.Queries.Projects;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace AuctionSystem.Presentation.Controllers
{
    /// <summary>
    /// Controller لإدارة المشاريع (Projects)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(Roles = "BuyerUser")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// استرجاع كل المشاريع
        /// </summary>
        /// <returns>قائمة بكل المشاريع</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllProjectsQuery());
            return Ok(result);
        }

        /// <summary>
        /// استرجاع مشروع حسب معرفه
        /// </summary>
        /// <param name="id">معرف المشروع</param>
        /// <returns>تفاصيل المشروع أو NotFound إذا لم يكن موجود</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _mediator.Send(new GetProjectByIdQuery(id));
            if (dto == null) return NotFound("Project not found");
            return Ok(dto);
        }

        /// <summary>
        /// إنشاء مشروع جديد
        /// </summary>
        /// <param name="cmd">بيانات المشروع الجديد</param>
        /// <returns>معرف المشروع الذي تم إنشاؤه</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectCommand cmd)
        {
            var newId = await _mediator.Send(cmd);
            return Ok(new { Success = true, ProjectId = newId });
        }

        /// <summary>
        /// استرجاع المشاريع حسب الحالة
        /// </summary>
        /// <param name="state">حالة المشروع (مثل Pending, Active, Completed)</param>
        /// <returns>قائمة المشاريع المطابقة للحالة</returns>
        [HttpGet("state/{state}")]
        public async Task<IActionResult> GetByState(ProjectStatus state)
        {
            var dtos = await _mediator.Send(new GetProjectByStateQuery(state));
            return Ok(dtos);
        }

        /// <summary>
        /// حذف مشروع
        /// </summary>
        /// <param name="id">معرف المشروع</param>
        /// <returns>نتيجة الحذف أو رسالة خطأ إذا لم يكن موجود</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var result = await _mediator.Send(new DeleteProjectCommand(id));
            return result ? Ok(new { Success = true, Message = "Project deleted" })
                          : NotFound(new { Success = false, Message = "Project not found" });
        }

        /// <summary>
        /// تقديم المشروع (Submit)
        /// </summary>
        /// <param name="id">معرف المشروع</param>
        /// <returns>نتيجة تقديم المشروع أو رسالة خطأ</returns>
        [HttpPost("{id}/submit")]
        public async Task<IActionResult> SubmitProject(int id)
        {
            var result = await _mediator.Send(new SubmitProjectCommand(id));
            if (!result.Success) return BadRequest(new { result.Success, result.ErrorMessage });
            return Ok(result);
        }
    }
}
