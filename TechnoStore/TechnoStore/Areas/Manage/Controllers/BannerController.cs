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
	public class BannerController : Controller
    {
        private DataContext _dataContext;
        private IWebHostEnvironment _env;

        public BannerController(DataContext dataContext,IWebHostEnvironment env)
        {
            _dataContext = dataContext;
            _env = env;
        }
        public IActionResult Index()
        {
            List<HomePageBanner> banners = _dataContext.HomePageBanners.ToList();

            return View(banners);
        }

   //     public IActionResult Create()
   //     {
   //         return View();
   //     }

   //     [HttpPost]
   //     public IActionResult Create(HomePageBannerVM bannerVM)
   //     {
   //         if(!ModelState.IsValid) return View(bannerVM);

   //         if (bannerVM.ImageFile == null) return View(bannerVM);

   //         if (bannerVM.ImageFile.ContentType != "image/png" && bannerVM.ImageFile.ContentType != "image/jpeg" && bannerVM.ImageFile.ContentType != "image/jpg")
   //         {
   //             ModelState.AddModelError("ImageFile", "Yalniz Shekil fayli ola biler!");
   //             return View(bannerVM);
   //         }
   //         if (bannerVM.ImageFile.Length > 3145728)
   //         {
   //             ModelState.AddModelError("ImageFile", "Faylin ölçüsü max 3 mb ola biler!");
   //             return View(bannerVM);
   //         }

   //         HomePageBanner newBanner = new HomePageBanner
			//{
   //             Url = bannerVM.Url,
   //             Image = FileManager.FileSave(_env.WebRootPath, "uploads\\banners", bannerVM.ImageFile),
   //             Name = bannerVM.Name,
   //             Disable = bannerVM.Disable,
   //         };

   //         _dataContext.HomePageBanners.Add(newBanner);
   //         _dataContext.SaveChanges();

   //         return RedirectToAction("Index");
   //     }

        public IActionResult Update(int id)
        {
            HomePageBanner banner = _dataContext.HomePageBanners.FirstOrDefault(x=>x.Id == id);

            if(banner == null) return View("Error");

            HomePageBannerVM bannerVM = new HomePageBannerVM
            {
                Id = id,
                Url = banner.Url,
                Image = banner.Image,
                Disable = banner.Disable,
               
            };

            return View(bannerVM);
        }

        [HttpPost]
        public IActionResult Update(HomePageBannerVM bannerVM)
        {
            HomePageBanner existBanner = _dataContext.HomePageBanners.FirstOrDefault(x => x.Id == bannerVM.Id);

            if(existBanner == null) return View("Error");

            if (bannerVM.ImageFile != null)
            {
                if (bannerVM.ImageFile.ContentType != "image/png" && bannerVM.ImageFile.ContentType != "image/jpeg" && bannerVM.ImageFile.ContentType != "image/jpg")
                {
                    ModelState.AddModelError("ImageFile", "Yalniz Shekil fayli ola biler!");
                    return View(bannerVM);
                }
                if (bannerVM.ImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile", "Faylin ölçüsü max 3 mb ola biler!");
                    return View(bannerVM);
                }

                FileManager.FileDelete(_env.WebRootPath, "uploads\\banners", existBanner.Image);

                existBanner.Image = FileManager.FileSave(_env.WebRootPath, "uploads\\banners", bannerVM.ImageFile);
            }


            existBanner.Url = bannerVM.Url;
            

            _dataContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var deleteBanner = _dataContext.HomePageBanners.FirstOrDefault(x => x.Id == id);

            if (deleteBanner == null) return View("Error");

            FileManager.FileDelete(_env.WebRootPath, "uploads\\banners", deleteBanner.Image);

            _dataContext.HomePageBanners.Remove(deleteBanner);
            _dataContext.SaveChanges();

            return View(nameof(Index));

        }
    }
}
