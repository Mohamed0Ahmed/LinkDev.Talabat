using LinkDev.Talabat.Application.Abstraction.Interfaces;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Talabat.Dashboard.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggedInUserService _loggedUserService;

        public CategoryController(IUnitOfWork unitOfWork, ILoggedInUserService loggedUserService)
        {
            _unitOfWork = unitOfWork;
            _loggedUserService = loggedUserService;
        }


        // GET: Category/Index
        public async Task<IActionResult> Index()
        {
            var brands = await _unitOfWork.GetRepository<ProductCategory, int>().GetAllAsync();
            return View(brands);
        }

        // POST: Category/Create
        [HttpPost]
        public async Task<JsonResult> Create(string Name)
        {
            var productCategory = new ProductCategory
            {
                Id = 0,
                Name = Name,
                CreatedBy = _loggedUserService.UserId ?? "System",
                LastModifiedBy = _loggedUserService.UserId ?? "System"
            };

            if (string.IsNullOrEmpty(productCategory.Name) || productCategory.Name.Length > 100)
            {
                return Json(new { success = false, message = "Invalid data: Category name is required and can't be longer than 100 characters." });
            }

            try
            {
                // Add the new Category to the repository
                await _unitOfWork.GetRepository<ProductCategory, int>().AddAsync(productCategory);
                await _unitOfWork.CompleteAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Category/Delete/5
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var productCategory = await _unitOfWork.GetRepository<ProductCategory, int>().GetAsync(id);
            if (productCategory == null)
            {
                return Json(new { success = false, message = "Category not found." });
            }

            try
            {
                _unitOfWork.GetRepository<ProductCategory, int>().Delete(productCategory);
                await _unitOfWork.CompleteAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
