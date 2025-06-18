using Kafo.DAL.Repository;
using KAFO.BLL.Managers;
using KAFO.Domain.Products;
using Microsoft.AspNetCore.Mvc;

namespace KAFO.ASPMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProductManager productManager;

        public ProductManagementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            productManager = new ProductManager(unitOfWork);
        }

        // GET: ProductController
        [HttpGet]
        public ActionResult Index()
        {
            var Products = productManager.GetAll(includeProperties: "Category");
            return View("SimpleTest");
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();
            var Product = productManager.Get(p => p.Id == id, "Category");

            return View("~/Areas/Admin/Views/ProductManagement/Index.cshtml");
        }
        [HttpGet]
        // GET: ProductController/Create
        public ActionResult Create()
        {
            try
            {
                var Categories = _unitOfWork.Category.GetAll();
                ViewBag.Categories = Categories;
                return View();

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    product.ImageUrl = await UploadFile(imageFile, product.Name);

                    //BLL MAke LastPurchasing = AVGPurchasing
                    productManager.Add(product);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Create();
                }
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            var Categories = _unitOfWork.Category.GetAll();
            var Product = productManager.Get(p => p.Id == id, "Category");
            ViewBag.Categories = Categories;
            return View(Product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Product product, IFormFile? imageFile)
        {
            if (id == null)
                return BadRequest();
            try
            {
                if (ModelState.IsValid)
                {

                    product.ImageUrl = await UploadFile(imageFile, product.Name);
                    productManager.AddORUpdate((int)id, product);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Edit(id);
                }
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var Product = productManager.Get(p => p.Id == id, "Category");

            return View(Product);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
                return BadRequest();
            try
            {
                var p = productManager.Get(p => p.Id == id);
                if (p != null)
                {
                    productManager.Remove(p);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        private async Task<string?> UploadFile(IFormFile? imageFile, string productName)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Upload/products");
                Directory.CreateDirectory(uploadsFolder); // make sure folder exists
                //uniqe => productName only
                var fileName = $"[{productName}] - {DateTime.Now.ToString("dd-MM-yyy-HH-mm-ss")}{Path.GetExtension(imageFile.FileName)}"; // safer filename
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Save image URL in your model
                return $"/images/Upload/products/{fileName}";
            }
            return null;
        }
    }
}
