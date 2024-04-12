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
        public IActionResult Create()
        {
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category
            //   .GetAll().Select(u => new SelectListItem
            //   {
            //       Text = u.Name,
            //       Value = u.Id.ToString()
            //   });
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
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
            return View(productVM);
        }
        [HttpPost]
        //public IActionResult Create(Product obj)
        public IActionResult Create(ProductVM productVM)
        {
            
            if (ModelState.IsValid)
            {
                //_db.Categories.Add(obj); //what it has to add
                //_db.SaveChanges();  //go to the database and create the product
                //_productRepo.Add(obj); 
                //_productRepo.Save();

                //_unitOfWork.Product.Add(obj);
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
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Product? productFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);
            //Product? productFromDb1 = _db.Categories.Find(id);
            //Product? productFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault;

            //Product? productFromDb = _productRepo.Get(u=> u.Id==id);
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {

            if (ModelState.IsValid)
            {
                //_db.Categories.Update(obj); //what it has to add
                //_db.SaveChanges();       //go to the database and create the product
                //_productRepo.Update(obj);
                //_productRepo.Save();
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            return View();
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
