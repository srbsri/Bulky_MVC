using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _db;
        //private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;

        //public CategoryController(ApplicationDbContext db)
        //public CategoryController(ICategoryRepository db)
        public CategoryController(IUnitOfWork unitOfWork)
        {
            //_db = db;
            //_categoryRepo= db;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();

            return View(objCategoryList);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "Display Order  & name cannot be same");
            }
            if (obj.DisplayOrder.ToString() == "7")
            {
                ModelState.AddModelError("", "7 is a invalid value");
            }
            if (ModelState.IsValid)
            {
                //_db.Categories.Add(obj); //what it has to add
                //_db.SaveChanges();  //go to the database and create the category
                //_categoryRepo.Add(obj); 
                //_categoryRepo.Save();
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Category? categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);
            //Category? categoryFromDb1 = _db.Categories.Find(id);
            //Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault;

            //Category? categoryFromDb = _categoryRepo.Get(u=> u.Id==id);
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                //_db.Categories.Update(obj); //what it has to add
                //_db.SaveChanges();       //go to the database and create the category
                //_categoryRepo.Update(obj);
                //_categoryRepo.Save();
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
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
            //Category? categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);
            //Category? categoryFromDb = _categoryRepo.Get(u => u.Id == id);
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            //Category? obj = _db.Categories.FirstOrDefault(obj => obj.Id == id);
            //Category? obj = _categoryRepo.Get(u => u.Id == id);
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            //_db.Categories.Remove(obj);
            //_db.SaveChanges();
            //_categoryRepo.Remove(obj);
            //_categoryRepo.Save();
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
