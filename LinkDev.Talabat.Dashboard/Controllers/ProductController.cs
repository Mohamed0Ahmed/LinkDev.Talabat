using AutoMapper;
using LinkDev.Talabat.Application.Abstraction.Interfaces;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Products;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Core.Domain.Specifications.Products;
using LinkDev.Talabat.Dashboard.Helper;
using LinkDev.Talabat.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Route.Talabat.Dashboard.Models.LinkDev.Talabat.Dashboard.Models;

namespace LinkDev.Talabat.Dashboard.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggedInUserService _LoggedInUserService;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper, ILoggedInUserService loggedUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _LoggedInUserService = loggedUserService;
        }

        public async Task<IActionResult> Index(ProductSpecParams specParams)
        {
            var spec = new ProductWithBrandCategoryAndSortSpecification(specParams.Sort, specParams.BrandId, specParams.CategoryId, specParams.PageIndex, specParams.PageSize, specParams.Search);

            var products = await _unitOfWork.GetRepository<Product, int>().GetAllWithSpecAsync(spec);

            var totalItem = await _unitOfWork.GetRepository<Product, int>().GetCountAsync(new ProductWithBrandCategoryAndSortSpecification(
                specParams.Sort, specParams.BrandId, specParams.CategoryId, 1, int.MaxValue, specParams.Search
            ));

            var mappedProducts = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products);

            var viewModel = new PaginatedProductViewModel
            {
                Products = mappedProducts,
                PageIndex = specParams.PageIndex,
                PageSize = specParams.PageSize,
                TotalCount = totalItem
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Brands = new SelectList(await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _unitOfWork.GetRepository<ProductCategory, int>().GetAllAsync(), "Id", "Name");

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(model);

                product.NormalizedName = model.Name.ToUpperInvariant();

                product.CreatedBy = _LoggedInUserService.UserId;
                product.LastModifiedBy = _LoggedInUserService.UserId;

                product.PictureUrl = model.Image != null ? await SaveImage(model.Image) : null;

                await _unitOfWork.GetRepository<Product, int>().AddAsync(product);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction("Index");
            }


            ViewBag.Brands = new SelectList(await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _unitOfWork.GetRepository<ProductCategory, int>().GetAllAsync(), "Id", "Name");

            return View(model);
        }

        #region Helper
        private async Task<string> SaveImage(IFormFile imageFile)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return "/images/" + fileName;
            }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Brands = new SelectList(await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _unitOfWork.GetRepository<ProductCategory, int>().GetAllAsync(), "Id", "Name");

            var product = await _unitOfWork.GetRepository<Product, int>().GetAsync(id);
            var mappedProduct = _mapper.Map<Product, ProductViewModel>(product!);
            mappedProduct.PictureUrl = product?.PictureUrl;

            return View(mappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (model.Image is not null)
                {
                    if (!string.IsNullOrEmpty(model.PictureUrl))
                    {
                        var fileName = Path.GetFileName(model.PictureUrl);
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            PictureSettings.DeleteFile("products", fileName);
                        }
                    }

                    model.PictureUrl = PictureSettings.UploadFile(model.Image!, "products");
                }
                else if (string.IsNullOrEmpty(model.PictureUrl))
                {
                    ModelState.AddModelError("PictureUrl", "Image is required.");

                    ViewBag.Brands = new SelectList(await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync(), "Id", "Name");
                    ViewBag.Categories = new SelectList(await _unitOfWork.GetRepository<ProductCategory, int>().GetAllAsync(), "Id", "Name");
                    return View(model);
                }

                var mappedProduct = _mapper.Map<ProductViewModel, Product>(model);
                mappedProduct.CreatedBy = _LoggedInUserService.UserId;
                mappedProduct.LastModifiedBy = _LoggedInUserService.UserId;
                mappedProduct.NormalizedName = model.Name.ToUpperInvariant();

                _unitOfWork.GetRepository<Product, int>().Update(mappedProduct);

                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    return RedirectToAction("Index");
                }
            }

            // Refill dropdown lists 
            ViewBag.Brands = new SelectList(await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _unitOfWork.GetRepository<ProductCategory, int>().GetAllAsync(), "Id", "Name");

            return View(model);
        }




        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetAsync(id);
            var mappedProduct = _mapper.Map<Product, ProductViewModel>(product!);

            return View(mappedProduct);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id, ProductViewModel model)
        {
            try
            {
                if (id != model.Id)
                {
                    return NotFound();
                }

                var product = await _unitOfWork.GetRepository<Product, int>().GetAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(product.PictureUrl))
                {
                    var fileName = Path.GetFileName(product.PictureUrl);
                    PictureSettings.DeleteFile("products", fileName);
                }

                _unitOfWork.GetRepository<Product, int>().Delete(product);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                ModelState.AddModelError("", "An error occurred while deleting the product.");
                return View("Error");
            }
        }

    }
}
