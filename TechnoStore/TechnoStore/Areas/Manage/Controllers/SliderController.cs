using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechnoStore.Areas.Manage.ViewModels;
using TechnoStore.Businees;
using TechnoStore.Helpers;
using TechnoStore.Models;
using TechnoStore.Models.DataContext;

namespace TechnoStore.Areas.Manage.Controllers
{
    [Area("Manage")]
	[Authorize(Roles = "SuperAdmin,Admin")]
	public class SliderController : Controller
    {
        private DataContext _dataContext;
        private IWebHostEnvironment _env;
		private IMapper _mapper;

		public SliderController(DataContext dataContext, IWebHostEnvironment env,IMapper mapper)
        {
            _dataContext = dataContext;
            _env = env;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            List<Slider> sliderList = _dataContext.Sliders.ToList();
            return View(sliderList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SliderViewModel sliderVM)
        {
            if (!ModelState.IsValid) return View(sliderVM);

            if (sliderVM.ImageFile is null) return View(sliderVM);

            if (sliderVM.ImageFile.ContentType != "image/png" && sliderVM.ImageFile.ContentType != "image/jpeg" && sliderVM.ImageFile.ContentType != "image/jpg")
            {
                ModelState.AddModelError("ImageFile", "Yalniz Shekil fayli ola biler!");
                return View(sliderVM);
            }
            if (sliderVM.ImageFile.Length > 3145728)
            {
                ModelState.AddModelError("ImageFile", "Faylin ölçüsü max 3 mb ola biler!");
                return View(sliderVM);
            }

            //Slider newSlider = new Slider
            //{
            //    Name1 = sliderVM.Name1,
            //    Name2 = sliderVM.Name2,
            //    Description = sliderVM.Description,
            //    CreatedTime = DateTime.UtcNow.AddHours(4),
            //    Image = FileManager.FileSave(_env.WebRootPath, "uploads\\sliders", sliderVM.ImageFile),
            //    SellPrice = sliderVM.SellPrice,
            //    CostPrice = sliderVM.CostPrice,
            //    DiscountPrice = sliderVM.DiscountPrice,
            //    RedirectUrl = sliderVM.RedirectUrl,
            //    RedirectUrlText = sliderVM.RedirectUrlText,
            //    Header = sliderVM.Header
            //};

            //Slider newSlider = (Slider)sliderVM;
            //Slider newSlider = TypeConvert.Convertion<SliderViewModel, Slider>(sliderVM);
            Slider newSlider = _mapper.Map<Slider>(sliderVM);
            newSlider.CreatedTime = DateTime.UtcNow.AddHours(4);
            newSlider.Image = FileManager.FileSave(_env.WebRootPath, "uploads\\sliders", sliderVM.ImageFile);
            _dataContext.Sliders.Add(newSlider);
            _dataContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            Slider slider = _dataContext.Sliders.FirstOrDefault(x => x.Id == id);

            if (slider == null) return View("Error");

            //SliderViewModel sliderVM = new SliderViewModel
            //{
            //    Id = slider.Id,
            //    Name1 = slider.Name1,
            //    Name2 = slider.Name2,
            //    Description = slider.Description,
            //    Image = slider.Image,
            //    SellPrice = slider.SellPrice,
            //    CostPrice = slider.CostPrice,
            //    DiscountPrice = slider.DiscountPrice,
            //    RedirectUrl = slider.RedirectUrl,
            //    RedirectUrlText = slider.RedirectUrlText,
            //    Header = slider.Header
            //};

            SliderViewModel sliderVM = _mapper.Map<SliderViewModel>(slider);

            return View(sliderVM);
        }

        [HttpPost]
        public IActionResult Update(SliderViewModel sliderVM)
        {
            Slider existSlider = _dataContext.Sliders.FirstOrDefault(x => x.Id == sliderVM.Id);
            if (existSlider is null) return View("Error");

            if (!ModelState.IsValid) return View(sliderVM);

            if (sliderVM.ImageFile != null)
            {
                if (sliderVM.ImageFile.ContentType != "image/png" && sliderVM.ImageFile.ContentType != "image/jpeg" && sliderVM.ImageFile.ContentType != "image/jpg")
                {
                    ModelState.AddModelError("ImageFile", "Yalniz Shekil fayli ola biler!");
                    return View(sliderVM);
                }
                if (sliderVM.ImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile", "Faylin ölçüsü max 3 mb ola biler!");
                    return View(sliderVM);
                }

                FileManager.FileDelete(_env.WebRootPath, "uploads\\sliders", existSlider.Image);

                existSlider.Image = FileManager.FileSave(_env.WebRootPath, "uploads\\sliders", sliderVM.ImageFile);
            }

   //         existSlider.Name1 = sliderVM.Name1;
			//existSlider.Name2 = sliderVM.Name2;
			//existSlider.Description = sliderVM.Description;
			//existSlider.SellPrice = sliderVM.SellPrice;
			//existSlider.CostPrice = sliderVM.CostPrice;
			//existSlider.DiscountPrice = sliderVM.DiscountPrice;
			//existSlider.RedirectUrl = sliderVM.RedirectUrl;
			//existSlider.RedirectUrlText = sliderVM.RedirectUrlText;
   //         existSlider.Header = sliderVM.Header;
            
            existSlider = TypeConvert.Convertion<SliderViewModel,Slider>(sliderVM,existSlider);

			existSlider.LastUpdate = DateTime.UtcNow.AddHours(4);
            _dataContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            Slider deleteSlider = _dataContext.Sliders.FirstOrDefault(x => x.Id == id);
            if (deleteSlider is null) return View("Error");

            FileManager.FileDelete(_env.WebRootPath, "uploads\\sliders", deleteSlider.Image);

            _dataContext.Sliders.Remove(deleteSlider);
            _dataContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
