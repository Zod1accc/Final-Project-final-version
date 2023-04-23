using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
//using Org.BouncyCastle.Bcpg.Sig;
using TechnoStore.Areas.Manage.ViewModels;
using TechnoStore.Helpers;
using TechnoStore.Models;
using TechnoStore.Models.DataContext;

namespace TechnoStore.Areas.Manage.Controllers
{
	[Area("Manage")]
	[Authorize(Roles = "SuperAdmin,Admin")]
	public class TvController : Controller
	{
        private DataContext _dataContext;
        private IWebHostEnvironment _env;

        public TvController(DataContext dataContext,IWebHostEnvironment env)
		{
			_dataContext = dataContext;
			_env = env;
		}
		public IActionResult Index(int page=1)
		{
			//List<Product> products = _dataContext.Products
			//				.Where(x => x.Category.Name.ToLower() == "tv")
			//				.Include(x=>x.Images).ToList();

			var query = _dataContext.Products
							.Where(x => x.Category.Name.ToLower() == "tv")
							.Include(x => x.Images).AsQueryable();
            var paginatedProducts = PaginatedList<Product>.Create(query, 10, page);

			return View(paginatedProducts);
		}

		public IActionResult Create()
		{
			ViewBag.Brands = _dataContext.Brands.ToList();
			return View();
		}

        [HttpPost]
		public IActionResult Create(TvViewModel tv)
		{

			ViewBag.Brands = _dataContext.Brands.ToList();

            if (!ModelState.IsValid) return View(tv);


			if (tv.ImageFiles == null || tv.PosterImage == null) return View(tv);

            Product newProduct = new Product
            {
                Category = _dataContext.Categories.FirstOrDefault(x => x.Name.ToLower() == "tv"),
                Name = tv.Name,
                Description = tv.Description,
                SellCount = 0,
                SellPrice = tv.SellPrice,
                CostPrice = tv.CostPrice,
                DiscountPrice = tv.DiscountPrice,
                IsAvailablity = tv.IsAvailablity,
                IsFeatured = tv.IsFeatured,
                IsNew = tv.IsNew,
                BrandId = tv.BrandId,
                ProdutCount = tv.ProductCount
            };

            _dataContext.Products.Add(newProduct);

            if (tv.PosterImage.ContentType != "image/png" && tv.PosterImage.ContentType != "image/jpeg" && tv.PosterImage.ContentType != "image/jpg")
            {
                ModelState.AddModelError("PosterImage", "Yalniz Shekil fayli ola biler!");
                return View(tv);
            }
            if (tv.PosterImage.Length > 3145728)
            {
                ModelState.AddModelError("PosterImage", "Faylin ölçüsü max 3 mb ola biler!");
                return View(tv);
            }

            ProductImage productImage = new ProductImage
            {
                Product = newProduct,
                IsPoster = true,
                Image = FileManager.FileSave(_env.WebRootPath, "uploads\\products", tv.PosterImage)
            };

            _dataContext.ProductImages.Add(productImage);

            foreach (var img in tv.ImageFiles)
            {
                if (img.ContentType != "image/png" && img.ContentType != "image/jpeg" && img.ContentType != "image/jpg")
                {
                    ModelState.AddModelError("ImageFiles", "Yalniz Shekil fayli ola biler!");
                    return View(tv);
                }
                if (img.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFiles", "Faylin ölçüsü max 3 mb ola biler!");
                    return View(tv);
                }

                ProductImage productImages = new ProductImage
                {
                    Product = newProduct,
                    IsPoster = false,
                    Image = FileManager.FileSave(_env.WebRootPath, "uploads\\products", img)
                };

                _dataContext.ProductImages.Add(productImages);
            }

            Features features = new Features
            {
                Product = newProduct,
                EkranNovu = tv.EkranNovu,
                EkranIcazesi = tv.EkranIcazesi,
                Tezlik = tv.Tezlik,
                SesSistemi = tv.SesSistemi,
                IşığınNövü = tv.IşığınNövü,
				Cheki = tv.Cheki,
                Olchu = tv.Olchu,
				İstehsalcı = tv.İstehsalcı
			};

            _dataContext.Features.Add(features);
            _dataContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            Product product = _dataContext.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == id);
            Features features = _dataContext.Features.FirstOrDefault(x=>x.ProductId == id);
            if (product == null) return NotFound();

			ViewBag.Brands = _dataContext.Brands.ToList();

			ViewBag.PosterImage = product.Images.Where(x => x.IsPoster == true).FirstOrDefault();
            ViewBag.Images = product.Images.Where(x => x.IsPoster == false).ToList();

            TvViewModel tvVM = new TvViewModel
            {
                Id = id,
                Name = product.Name,
                SellPrice = product.SellPrice,
                CostPrice = product.CostPrice,
                DiscountPrice = product.DiscountPrice,
                //Images = product.Images,
                IsFeatured = product.IsFeatured,
                IsAvailablity = product.IsAvailablity,
                Description = product.Description,
                IsNew = product.IsNew,
                BrandId = product.BrandId,
                ProductCount = product.ProdutCount,

                EkranNovu = features.EkranNovu,
                EkranIcazesi = features.EkranIcazesi,
                Tezlik = features.Tezlik,
                SesSistemi = features.SesSistemi,
                IşığınNövü = features.IşığınNövü,
                Cheki = features.Cheki,
                Olchu = features.Olchu,
                İstehsalcı = features.İstehsalcı

            };


            return View(tvVM);
        }

        [HttpPost]
        public IActionResult Update(TvViewModel tv)
        {
			Product existProduct = _dataContext.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == tv.Id);

			ViewBag.Brands = _dataContext.Brands.ToList();
			ViewBag.PosterImage = existProduct.Images.FirstOrDefault(x => x.IsPoster == true);
			ViewBag.Images = existProduct.Images.Where(x => x.IsPoster == false).ToList();

			if (!ModelState.IsValid) return View();

            if (existProduct == null) return NotFound();
            Features existFeatures = _dataContext.Features.FirstOrDefault(x => x.ProductId == existProduct.Id);
			if (existFeatures == null) return NotFound();


			

            if (tv.ImageIds != null)
            {
                existProduct.Images.RemoveAll(img => !tv.ImageIds.Contains(img.Id) && img.IsPoster == false);

            }

            if (tv.PosterImage != null)
            {
                if (tv.PosterImage.ContentType != "image/png" && tv.PosterImage.ContentType != "image/jpeg" && tv.PosterImage.ContentType != "image/jpg")
                {
                    ModelState.AddModelError("PosterImage", "Yalniz Shekil fayli ola biler!");
                    return View(tv);
                }
                if (tv.PosterImage.Length > 3145728)
                {
                    ModelState.AddModelError("PosterImage", "Faylin ölçüsü max 3 mb ola biler!");
                    return View(tv);
                }
                if (existProduct.Images != null)
                {
                    var postImage = existProduct.Images.FirstOrDefault(x => x.IsPoster == true);
                    FileManager.FileDelete(_env.WebRootPath, "uploads\\products", postImage.Image);
                    existProduct.Images.Remove(postImage);
                }


                ProductImage productImage = new ProductImage
                {
                    Product = existProduct,
                    IsPoster = true,
                    Image = FileManager.FileSave(_env.WebRootPath, "uploads\\products", tv.PosterImage)
                };

                _dataContext.ProductImages.Add(productImage);
            }

            if (tv.ImageFiles != null)
            {
                foreach (var image in tv.ImageFiles)
                {
                    ProductImage Image = new ProductImage
                    {
                        Product = existProduct,
                        IsPoster = false,
                        Image = FileManager.FileSave(_env.WebRootPath, "uploads\\products", image)
                    };

                    _dataContext.ProductImages.Add(Image);
                }
            }



            existProduct.Name = tv.Name;
            existProduct.SellPrice = tv.SellPrice;
            existProduct.CostPrice = tv.CostPrice;
            existProduct.DiscountPrice = tv.DiscountPrice;
            existProduct.IsAvailablity = tv.IsAvailablity;
            existProduct.IsFeatured = tv.IsFeatured;
            existProduct.Description = tv.Description;
            existProduct.IsNew = tv.IsNew;
            existProduct.BrandId = tv.BrandId;
            existProduct.ProdutCount = tv.ProductCount;

			existFeatures.EkranNovu = tv.EkranNovu;
			existFeatures.EkranIcazesi = tv.EkranIcazesi;
			existFeatures.Tezlik = tv.Tezlik;
			existFeatures.SesSistemi = tv.SesSistemi;
			existFeatures.IşığınNövü = tv.IşığınNövü;
			existFeatures.Cheki = tv.Cheki;
			existFeatures.Olchu = tv.Olchu;
            existFeatures.İstehsalcı = tv.İstehsalcı;


			_dataContext.SaveChanges();

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int id)
        {
            Product deleteProduct = _dataContext.Products.FirstOrDefault(x => x.Id == id);
            if (deleteProduct == null) return NotFound();

            Features features = _dataContext.Features.FirstOrDefault(x => x.ProductId == id);

            var imagesList = _dataContext.ProductImages.Where(x => x.ProductId == deleteProduct.Id);
            if(imagesList != null)
            {
				foreach (var image in imagesList)
				{
                    FileManager.FileDelete(_env.WebRootPath, "uploads\\products", image.Image);
                    _dataContext.ProductImages.Remove(image);
				}
			}

            if(features is not null)
            {
				_dataContext.Features.Remove(features);
			}
			_dataContext.Products.Remove(deleteProduct);
            _dataContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
