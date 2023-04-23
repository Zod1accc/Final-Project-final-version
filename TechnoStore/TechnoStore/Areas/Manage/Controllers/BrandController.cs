using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechnoStore.Helpers;
using TechnoStore.Models;
using TechnoStore.Models.DataContext;

namespace TechnoStore.Areas.Manage.Controllers
{
	[Area("Manage")]
	[Authorize(Roles = "SuperAdmin,Admin")]
	public class BrandController : Controller
	{
		private DataContext _dataContext;

		public BrandController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public IActionResult Index(int page=1)
		{
			//List<Brand> brands = _dataContext.Brands.ToList();
			var query = _dataContext.Brands.AsQueryable();

			var paginatedBrands = PaginatedList<Brand>.Create(query, 10, page);


            return View(paginatedBrands);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Brand brand)
		{
			if (!ModelState.IsValid) return View();

			Brand newBrand = new Brand
			{
				Name = brand.Name,
			};

			_dataContext.Brands.Add(newBrand);
			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Update(int id)
		{
			Brand brand = _dataContext.Brands.FirstOrDefault(x => x.Id == id);
			if (brand == null) return NotFound();

			return View(brand);
		}

		[HttpPost]
		public IActionResult Update(Brand brand)
		{
			if (!ModelState.IsValid) return View();

			Brand existBrand = _dataContext.Brands.FirstOrDefault(x=>x.Id == brand.Id);
			if (existBrand == null) return NotFound();

			existBrand.Name = brand.Name;
			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));

		}
		//----->   Delete Brand and Releated Products deleted

		//public IActionResult Delete(int id)
		//{
		//	Brand brand = _dataContext.Brands.FirstOrDefault(x => x.Id == id);
		//	if (brand == null) return View();

		//	List<Product> products = _dataContext.Products.Where(x => x.BrandId == id).ToList();
		//	foreach (Product product in products)
		//	{
		//		if (!_dataContext.Products.Any(x => x.Id == product.Id)) return View("Error");
		//		_dataContext.Remove(product);
		//	}

		//	_dataContext.Brands.Remove(brand);
		//	_dataContext.SaveChanges();

		//	return RedirectToAction(nameof(Index));
		//}

		//---->    Delete Brands

		//public IActionResult Delete(int id)
		//{
		//	Brand brand = _dataContext.Brands.FirstOrDefault(x=>x.Id ==id);
		//	if (brand == null) return View();

		//	_dataContext.Brands.Remove(brand);
		//	_dataContext.SaveChanges();

		//	return RedirectToAction(nameof(Index));
		//}
	}
}
