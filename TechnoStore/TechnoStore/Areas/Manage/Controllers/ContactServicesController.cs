using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TechnoStore.Areas.Manage.ViewModels;
using TechnoStore.Helpers;
using TechnoStore.Models;
using TechnoStore.Models.DataContext;

namespace TechnoStore.Areas.Manage.Controllers
{
	[Area("Manage")]
	[Authorize(Roles = "SuperAdmin,Admin")]
	public class ContactServicesController : Controller
	{
		private DataContext _dataContext;
		private IWebHostEnvironment _env;

		public ContactServicesController(DataContext dataContext,IWebHostEnvironment env)
		{
			_dataContext = dataContext;
			_env = env;
		}
		public IActionResult Index()
		{
			List<ContactService> contactServices = _dataContext.ContactServices.ToList();

			return View(contactServices);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(ContactServicesViewModel contactServicesVM)
		{
			if(!ModelState.IsValid) return View(contactServicesVM);

			if (contactServicesVM.Icon == null) return View(contactServicesVM);

			if (contactServicesVM.Icon.ContentType != "image/png" && contactServicesVM.Icon.ContentType != "image/jpeg" && contactServicesVM.Icon.ContentType != "image/jpg")
			{
				ModelState.AddModelError("ImageFiles", "Yalniz Shekil fayli ola biler!");
				return View(contactServicesVM);
			}
			if (contactServicesVM.Icon.Length > 3145728)
			{
				ModelState.AddModelError("ImageFiles", "Faylin ölçüsü max 3 mb ola biler!");
				return View(contactServicesVM);
			}

			ContactService newContactService = new ContactService
			{
				Title = contactServicesVM.Title,
				Content = contactServicesVM.Content,
				Icon = FileManager.FileSave(_env.WebRootPath, "uploads\\contactservices", contactServicesVM.Icon),
			};

			_dataContext.ContactServices.Add(newContactService);
			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));

		}

		public IActionResult Update(int id)
		{
			var contactService = _dataContext.ContactServices.FirstOrDefault(x => x.Id == id);
			if (contactService == null) return View();

			ContactServicesViewModel contactServicesVM = new ContactServicesViewModel
			{
				Id = id,
				IconStr = contactService.Icon,
				Title = contactService.Title,
				Content = contactService.Content,
			};

			return View(contactServicesVM);
		}

		[HttpPost]
		public IActionResult Update(ContactServicesViewModel contactServicesVM)
		{
			if (!ModelState.IsValid) return View(contactServicesVM);

			var existService = _dataContext.ContactServices.FirstOrDefault(x => x.Id == contactServicesVM.Id);
			if (existService == null) return NotFound();

			if(contactServicesVM.Icon != null)
			{
				if (contactServicesVM.Icon.ContentType != "image/png" && contactServicesVM.Icon.ContentType != "image/jpeg" && contactServicesVM.Icon.ContentType != "image/jpg")
				{
					ModelState.AddModelError("ImageFiles", "Yalniz Shekil fayli ola biler!");
					return View(contactServicesVM);
				}
				if (contactServicesVM.Icon.Length > 3145728)
				{
					ModelState.AddModelError("ImageFiles", "Faylin ölçüsü max 3 mb ola biler!");
					return View(contactServicesVM);
				}

				FileManager.FileDelete(_env.WebRootPath, "uploads\\contactservices", existService.Icon);

				existService.Icon = FileManager.FileSave(_env.WebRootPath, "uploads\\contactservices", contactServicesVM.Icon);
			}

			existService.Title = contactServicesVM.Title;
			existService.Content = contactServicesVM.Content;

			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int id)
		{
			var service = _dataContext.ContactServices.FirstOrDefault(x => x.Id == id);
			if(service == null) return NotFound();

			FileManager.FileDelete(_env.WebRootPath, "uploads\\contactservices", service.Icon);

			_dataContext.ContactServices.Remove(service);
			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult ContactCenterIndex()
		{
			var contactCenter = _dataContext.ContactCenters.ToList();

			return View(contactCenter);
		}

		public IActionResult ContactCenterUpdate(int id)
		{
			var contactCenter = _dataContext.ContactCenters.FirstOrDefault(x=>x.Id == id);
			if(contactCenter == null) return NotFound();

			ContactCenterVM contactCenterVM = new ContactCenterVM
			{
				Id = id,
				Value = contactCenter.Value,
			};

			return View(contactCenterVM);
		}

		[HttpPost]
		public IActionResult ContactCenterUpdate(ContactCenterVM contactCenterVM)
		{
			if (!ModelState.IsValid) return View(contactCenterVM);
			var contactCenter = _dataContext.ContactCenters.FirstOrDefault(x => x.Id == contactCenterVM.Id);
			if(contactCenter == null) return NotFound();

			contactCenter.Key = contactCenterVM.Key;
			contactCenterVM.Value = contactCenterVM.Value;

			return RedirectToAction(nameof(ContactCenterIndex));

		}

		public IActionResult Contacts()
		{
			var contacts = _dataContext.Contacts
									.Include(x=>x.AppUser).ToList();

			return View(contacts);
		}

		public IActionResult ContactsDetail(int id)
		{
			var contact = _dataContext.Contacts.FirstOrDefault(x => x.Id == id);
			if(contact == null) return NotFound();

			contact.Viewed = true;
			_dataContext.SaveChanges();

			return View(contact);
		}

		public IActionResult ContactDelete(int id)
		{
            var contact = _dataContext.Contacts.FirstOrDefault(x => x.Id == id);
            if (contact == null) return NotFound();

			_dataContext.Contacts.Remove(contact);
			_dataContext.SaveChanges();

			return Ok();
        }
	}
}
