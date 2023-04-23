using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnoStore.Models;
using TechnoStore.Models.DataContext;

namespace TechnoStore.Areas.Manage.Controllers
{
	[Area("Manage")]
	public class WidgetController : Controller
	{
		private DataContext _dataContext;

		public WidgetController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public IActionResult Index()
		{
			var widgetTitles = _dataContext.WidgetTitles.ToList();

			return View(widgetTitles);
		}

		//public IActionResult Create()
		//{
		//	return View();
		//}

		//[HttpPost]
		//public IActionResult Create(WidgetTitle widgetTitle)
		//{
		//	if(!ModelState.IsValid) return View();

		//	WidgetTitle newWidgetTitle = new WidgetTitle
		//	{
		//		Title = widgetTitle.Title,
		//	};

		//	_dataContext.WidgetTitles.Add(newWidgetTitle);
		//	_dataContext.SaveChanges();

		//	return RedirectToAction(nameof(Index));
		//}

		public IActionResult Update(int id)
		{
			var widgetTitle = _dataContext.WidgetTitles.FirstOrDefault(x => x.Id == id);
			if (widgetTitle is null) return NotFound();

			return View(widgetTitle);
		}

		[HttpPost]
		public IActionResult Update(WidgetTitle widgetTitle)
		{
			if (!ModelState.IsValid) return View();

			var existWidgetTitle = _dataContext.WidgetTitles.FirstOrDefault(x => x.Id == widgetTitle.Id);
			if (existWidgetTitle is null) return NotFound();

			existWidgetTitle.Title = widgetTitle.Title;

			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult WidgetItemIndex()
		{
			var widgetItems = _dataContext.WidgetItems.Include(x => x.WidgetTitle).ToList();	

			return View(widgetItems);
		}

		public IActionResult WidgetItemCreate()
		{
			ViewBag.WidgetTitles = _dataContext.WidgetTitles.ToList();
			return View();
		}

		[HttpPost]
		public IActionResult WidgetItemCreate(WidgetItem widgetItem)
		{
			ViewBag.WidgetTitles = _dataContext.WidgetTitles.ToList();
			if (!ModelState.IsValid) return View(widgetItem);

			WidgetItem newWidgetItem = new WidgetItem
			{
				Name = widgetItem.Name,
				RedirectUrl = widgetItem.RedirectUrl,
				WidgetTitleId = widgetItem.WidgetTitleId
			};

			_dataContext.WidgetItems.Add(newWidgetItem);
			_dataContext.SaveChanges();

			return RedirectToAction(nameof(WidgetItemIndex));

			
		}

		public IActionResult WidgetItemUpdate(int id)
		{
			ViewBag.WidgetTitles = _dataContext.WidgetTitles.ToList();
			var widgetItem =_dataContext.WidgetItems.FirstOrDefault(x=>x.Id == id);
			if(widgetItem is null) return NotFound();

			return View(widgetItem);
		}

		[HttpPost]
		public IActionResult WidgetItemUpdate(WidgetItem widgetItem)
		{
			ViewBag.WidgetTitles = _dataContext.WidgetTitles.ToList();
			if (!ModelState.IsValid) return View(widgetItem);

			var existWidgetItem = _dataContext.WidgetItems.FirstOrDefault(x => x.Id == widgetItem.Id);
			if (existWidgetItem is null) return NotFound();

			existWidgetItem.Name = widgetItem.Name;
			existWidgetItem.RedirectUrl = widgetItem.RedirectUrl;
			existWidgetItem.WidgetTitleId = widgetItem.WidgetTitleId;

			_dataContext.SaveChanges();

			return RedirectToAction(nameof(WidgetItemIndex));
		}

		public IActionResult Delete(int id)
		{
			var deleteWidgetItem = _dataContext.WidgetItems.FirstOrDefault(x => x.Id == id);
			if(deleteWidgetItem is null) return View(deleteWidgetItem);

			_dataContext.WidgetItems.Remove(deleteWidgetItem);
			_dataContext.SaveChanges();

			return RedirectToAction(nameof(WidgetItemIndex));
		}
	}
}
