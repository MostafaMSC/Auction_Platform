using Microsoft.AspNetCore.Mvc;
using MediatR;
using AuctionSystem.Application.Commands.Category;
using AuctionSystem.Application.Queries.Category;
using AuctionSystem.Application.Commands.Categories;
using Microsoft.AspNetCore.Authorization;

namespace AuctionSystem.Presentation.Controllers
{
    /// <summary>
    /// Controller لإدارة التصنيفات (Categories)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// إنشاء تصنيف جديد
        /// </summary>
        /// <param name="command">بيانات التصنيف الجديد</param>
        /// <returns>معرف التصنيف الذي تم إنشاؤه</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { Success = true, Id = id });
        }

        /// <summary>
        /// استرجاع كل التصنيفات
        /// </summary>
        /// <returns>قائمة بكل التصنيفات</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(list);
        }

        /// <summary>
        /// حذف تصنيف محدد
        /// </summary>
        /// <param name="id">معرف التصنيف الذي سيتم حذفه</param>
        /// <returns>نجاح العملية أو NotFound إذا لم يكن موجود</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteCategoryCommand(id));
            if (!success) return NotFound();
            return Ok(new { Success = true });
        }
    }
}
