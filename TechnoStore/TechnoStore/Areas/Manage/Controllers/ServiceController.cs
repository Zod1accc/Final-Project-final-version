using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TechnoStore.Areas.Manage.ViewModels;
using TechnoStore.Helpers;
using TechnoStore.Models;
using TechnoStore.Models.DataContext;

namespace TechnoStore.Areas.Manage.Controllers
{
	[Area("Manage")]
	[Authorize(Roles = "SuperAdmin,Admin")]
	public class ServiceController : Controller
	{
		private DataContext _dataContext;
		private IWebHostEnvironment _env;

		public ServiceController(DataContext dataContext,IWebHostEnvironment env)
		{
			_dataContext = dataContext;
			_env = env;
		}
		public IActionResult Index()
		{
			List<Service> servicesList = _dataContext.Services.ToList();

			return View(servicesList);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(ServiceViewModel serviceVM)
		{
			if(!ModelState.IsValid) return View(serviceVM);

			if(serviceVM.ImageFile == null) return View(serviceVM);

			if (serviceVM.ImageFile.ContentType != "image/png" && serviceVM.ImageFile.ContentType != "image/jpeg" && serviceVM.ImageFile.ContentType != "image/jpg")
			{
				ModelState.AddModelError("ImageFile", "Yalniz Shekil fayli ola biler!");
				return View(serviceVM);
			}
			if (serviceVM.ImageFile.Length > 3145728)
			{
				ModelState.AddModelError("ImageFile", "Faylin ölçüsü max 3 mb ola biler!");
				return View(serviceVM);
			}

			Service newService = new Service
			{
				Title = serviceVM.Title,
				Description = serviceVM.Description,
				Image = FileManager.FileSave(_env.WebRootPath, "uploads\\services", serviceVM.ImageFile)
			};

			_dataContext.Services.Add(newService);
			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));

		}

		public IActionResult Update(int id)
		{
			Service service = _dataContext.Services.FirstOrDefault(x => x.Id == id);

			if (service == null) return View("Error");

			ServiceViewModel serviceViewModel = new ServiceViewModel
			{
				Id = id,
				Title = service.Title,
				Description = service.Description,
				Image = service.Image
			};

			return View(serviceViewModel);
		}

		[HttpPost]
		public IActionResult Update(ServiceViewModel serviceVM)
		{
			Service existService = _dataContext.Services.FirstOrDefault(x => x.Id == serviceVM.Id);

			if (existService == null) return View("Error");

			if(serviceVM.ImageFile != null)
			{
				if (serviceVM.ImageFile.ContentType != "image/png" && serviceVM.ImageFile.ContentType != "image/jpeg" && serviceVM.ImageFile.ContentType != "image/jpg")
				{
					ModelState.AddModelError("ImageFile", "Yalniz Shekil fayli ola biler!");
					return View(serviceVM);
				}
				if (serviceVM.ImageFile.Length > 3145728)
				{
					ModelState.AddModelError("ImageFile", "Faylin ölçüsü max 3 mb ola biler!");
					return View(serviceVM);
				}

				FileManager.FileDelete(_env.WebRootPath, "uploads\\services", existService.Image);

				existService.Image = FileManager.FileSave(_env.WebRootPath, "uploads\\services", serviceVM.ImageFile);

			}

			existService.Title = serviceVM.Title;
			existService.Description =serviceVM.Description;

			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int id)
		{
			Service deleteServive = _dataContext.Services.FirstOrDefault(x => x.Id == id);

			if (deleteServive == null) return View("Error");

			FileManager.FileDelete(_env.WebRootPath, "uploads\\services", deleteServive.Image);

			_dataContext.Services.Remove(deleteServive);
			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}
	}
}
