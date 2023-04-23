using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechnoStore.Areas.Manage.ViewModels;
using TechnoStore.Helpers;
using TechnoStore.Models;
using TechnoStore.Models.DataContext;

namespace TechnoStore.Areas.Manage.Controllers
{
	[Area("Manage")]
	[Authorize(Roles = "SuperAdmin,Admin")]
	public class ZipcodeController : Controller
	{
		private DataContext _dataContext;

		public ZipcodeController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public IActionResult Index(int page=1)
		{
			var zipCodes = _dataContext.ZipCodes.ToList();

			var query = _dataContext.ZipCodes.AsQueryable();
			var paginatedZipCodes = PaginatedList<ZipCode>.Create(query, 5, page);

			return View(paginatedZipCodes);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(ZipCodeViewModel zipCodeVM)
		{
			if(!ModelState.IsValid) return View(zipCodeVM);
			var zipcode = _dataContext.ZipCodes.FirstOrDefault(x=>x.Code ==zipCodeVM.Code);
			if (zipcode is not null)
			{
				ModelState.AddModelError("Code", "Code has taken!");
				return View(zipCodeVM);
			}
			ZipCode newZipCode = new ZipCode
			{
				Code = zipCodeVM.Code,
				DiscountPrice = zipCodeVM.DiscountPrice,
				CreatedDate = DateTime.Now,
				ValidityPeriodInDays = zipCodeVM.ValidityPeriodInDays,
			};

			_dataContext.ZipCodes.Add(newZipCode);
			_dataContext.SaveChanges();

			return RedirectToAction("Index");

		}

		public IActionResult Update(int id)
		{
			var zipCode = _dataContext.ZipCodes.FirstOrDefault(x => x.Id == id);
			if (zipCode == null) return NotFound();

			ZipCodeViewModel zipCodeVM = new ZipCodeViewModel
			{
				Id = id,
				Code = zipCode.Code,
				DiscountPrice = zipCode.DiscountPrice,
				ValidityPeriodInDays = zipCode.ValidityPeriodInDays,
			};

			return View(zipCodeVM);
		}

		[HttpPost]
		public IActionResult Update(ZipCodeViewModel zipCodeVM)
		{
			if (!ModelState.IsValid) return View(zipCodeVM);

			var existZipCode = _dataContext.ZipCodes.FirstOrDefault(x => x.Id == zipCodeVM.Id);
			if(existZipCode == null) return NotFound();

			existZipCode.Code = zipCodeVM.Code;
			existZipCode.DiscountPrice = zipCodeVM.DiscountPrice;
			existZipCode.ValidityPeriodInDays = zipCodeVM.ValidityPeriodInDays;

			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int id)
		{
			var deleteZipCode = _dataContext.ZipCodes.FirstOrDefault(x=>x.Id == id);
			if(deleteZipCode == null) return NotFound();

			_dataContext.ZipCodes.Remove(deleteZipCode);
			_dataContext.SaveChanges();

			return Ok();
		}
	}
}
