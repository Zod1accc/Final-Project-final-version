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
	public class SettingController : Controller
	{
		private DataContext _dataContext;
		private IWebHostEnvironment _env;

		public SettingController(DataContext dataContext,IWebHostEnvironment env)
		{
			_dataContext = dataContext;
			_env = env;
		}
		public IActionResult Index()
		{
			List<Setting> settingList = _dataContext.Settings.ToList();
			
			return View(settingList);
		}

		public IActionResult Update(int id)
		{
			var setting = _dataContext.Settings.FirstOrDefault(x=>x.Id == id);
			if (setting == null) return NotFound();

			SettingViewModel settingVM = new SettingViewModel
			{
				Callus = setting.Callus,
				Address = setting.Address,
				CallusText = setting.CallusText,
				FacebookLink = setting.FacebookLink,
				Instagram = setting.Instagram,
				Google = setting.Google,
				TwitterLink = setting.TwitterLink,
				CallUsIcon = setting.CallusIcon,
				Logo = setting.Logo
			};
			return View(settingVM);
		}

		[HttpPost]
		public IActionResult Update(SettingViewModel settingVM)
		{
			var existSetting = _dataContext.Settings.FirstOrDefault();
			if (existSetting == null) return NotFound();

			if (settingVM.LogoImage != null)
			{
				if (settingVM.LogoImage.ContentType != "image/png" && settingVM.LogoImage.ContentType != "image/jpeg" && settingVM.LogoImage.ContentType != "image/jpg")
				{
					ModelState.AddModelError("LogoImage", "Yalniz Shekil fayli ola biler!");
					return View(settingVM);
				}
				if (settingVM.LogoImage.Length > 3145728)
				{
					ModelState.AddModelError("LogoImage", "Faylin ölçüsü max 3 mb ola biler!");
					return View(settingVM);
				}
				if (existSetting.Logo != null)
				{
					FileManager.FileDelete(_env.WebRootPath, "uploads\\settings", existSetting.Logo);
				}

				existSetting.Logo = FileManager.FileSave(_env.WebRootPath, "uploads\\settings", settingVM.LogoImage);

			}
			if (settingVM.CallusIcon != null)
			{
				if (settingVM.CallusIcon.ContentType != "image/png" && settingVM.CallusIcon.ContentType != "image/jpeg" && settingVM.CallusIcon.ContentType != "image/jpg")
				{
					ModelState.AddModelError("CallusIcon", "Yalniz Shekil fayli ola biler!");
					return View(settingVM);
				}
				if (settingVM.CallusIcon.Length > 3145728)
				{
					ModelState.AddModelError("CallusIcon", "Faylin ölçüsü max 3 mb ola biler!");
					return View(settingVM);
				}
				if (existSetting.CallusIcon != null)
				{
					FileManager.FileDelete(_env.WebRootPath, "uploads\\settings", existSetting.CallusIcon);
				}

				existSetting.CallusIcon = FileManager.FileSave(_env.WebRootPath, "uploads\\settings", settingVM.CallusIcon);

			}

			existSetting.Callus = settingVM.Callus;
			existSetting.Address = settingVM.Address;
			existSetting.CallusText = settingVM.CallusText;
			existSetting.FacebookLink = settingVM.FacebookLink;
			existSetting.Instagram = settingVM.Instagram;
			existSetting.Google = settingVM.Google;
			existSetting.TwitterLink = settingVM.TwitterLink;

			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}
	}
}
