using BulkyBook.Models;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Models.ViewModels;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        //private readonly ApplicationDbContext _db;
        //private readonly IProductRepository _productRepo;
        private readonly IUnitOfWork _unitOfWork;

        //public ProductController(ApplicationDbContext db)
        //public ProductController(IProductRepository db)
        public ProductController(IUnitOfWork unitOfWork)
        {
            //_db = db;
            //_productRepo= db;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //List<Product> objProductList = _productRepo.GetAll().ToList();
            List<BulkyBook.Models.Product> objProductList = _unitOfWork.Product.GetAll().ToList();

            return View(objProductList);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category
                   .GetAll().Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                // create
                return View(productVM);
            }
            else
            {
                //for update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }

        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                TempData["error"] = "There is some Error";
                return View(productVM);

            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Product? productFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);
            //Product? productFromDb = _productRepo.Get(u => u.Id == id);
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            //Product? obj = _db.Categories.FirstOrDefault(obj => obj.Id == id);
            //Product? obj = _productRepo.Get(u => u.Id == id);
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            //_db.Categories.Remove(obj);
            //_db.SaveChanges();
            //_productRepo.Remove(obj);
            //_productRepo.Save();
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
