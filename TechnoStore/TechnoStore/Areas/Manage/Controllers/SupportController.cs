using Microsoft.AspNetCore.Mvc;
using TechnoStore.Models;
using TechnoStore.Models.DataContext;

namespace TechnoStore.Areas.Manage.Controllers
{
	[Area("Manage")]
	public class SupportController : Controller
	{
		private DataContext _dataContext;

		public SupportController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public IActionResult Index()
		{
			List<Support> supportList = _dataContext.Supports.ToList();

			return View(supportList);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Support support)
		{
			if(!ModelState.IsValid) return View(support);

			Support newSuppport = new Support
			{
				Title = support.Title,
				Description = support.Description,
			};

			_dataContext.Supports.Add(newSuppport);
			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Update(int id)
		{
			Support support = _dataContext.Supports.FirstOrDefault(x=>x.Id == id);
			if (support == null) return NotFound();

			return View(support);
		}

		[HttpPost]
		public IActionResult Update(Support support)
		{
			if (!ModelState.IsValid) return View(support);

			Support existSupport = _dataContext.Supports.FirstOrDefault(x => x.Id == support.Id);
			if (existSupport == null) return NotFound();

			existSupport.Title = support.Title;
			existSupport.Description = support.Description;

			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int id)
		{
			Support support = _dataContext.Supports.FirstOrDefault(x => x.Id == id);
			if(support == null) return NotFound();

			_dataContext.Supports.Remove(support);
			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}
	}
}
