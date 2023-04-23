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
	public class PhoneController : Controller
	{
		private DataContext _dataContext;
		private IWebHostEnvironment _env;

		public PhoneController(DataContext dataContext,IWebHostEnvironment env)
		{
			_dataContext = dataContext;
			_env = env;
		}
		public IActionResult Index(int page=1)
		{
			//List<Product> products = _dataContext.Products
			//			.Include(x => x.Images)
			//			.Where(x => x.Category.Name.ToLower() == "phone").ToList(); 

			var query = _dataContext.Products
									.Include(x=>x.Images)
									.Where(x => x.Category.Name.ToLower() == "phone").AsQueryable();
			var paginatedProduct = PaginatedList<Product>.Create(query,10,page);

			return View(paginatedProduct);
		}

		public IActionResult Create()
		{
			ViewBag.Brands = _dataContext.Brands.ToList();

			return View();
		}

		[HttpPost]
		public IActionResult Create(PhoneViewModel phone)
		{
			ViewBag.Brands = _dataContext.Brands.ToList();

			if(!ModelState.IsValid) return View(phone);


			if (phone.ImageFiles == null || phone.PosterImage == null) return View(phone);

			Product newProduct = new Product
			{
				Category = _dataContext.Categories.FirstOrDefault(x => x.Name.ToLower() == "phone"),
				Name = phone.Name,
				Description = phone.Description,
				SellCount = 0,
				SellPrice = phone.SellPrice,
				CostPrice = phone.CostPrice,
				DiscountPrice = phone.DiscountPrice,
				IsAvailablity = phone.IsAvailablity,
				IsFeatured = phone.IsFeatured,
				BrandId = phone.BrandId,
				IsNew = phone.IsNew,
				ProdutCount = phone.ProductCount
			};

			_dataContext.Products.Add(newProduct);

			if (phone.PosterImage.ContentType != "image/png" && phone.PosterImage.ContentType != "image/jpeg" && phone.PosterImage.ContentType != "image/jpg")
			{
				ModelState.AddModelError("PosterImage", "Yalniz Shekil fayli ola biler!");
				return View(phone);
			}
			if (phone.PosterImage.Length > 3145728)
			{
				ModelState.AddModelError("PosterImage", "Faylin ölçüsü max 3 mb ola biler!");
				return View(phone);
			}

			ProductImage productImage = new ProductImage
			{
				Product = newProduct,
				IsPoster = true,
				Image = FileManager.FileSave(_env.WebRootPath, "uploads\\products", phone.PosterImage)
			};

			_dataContext.ProductImages.Add(productImage);

			foreach (var img in phone.ImageFiles)
			{
				if (img.ContentType != "image/png" && img.ContentType != "image/jpeg" && img.ContentType != "image/jpg")
				{
					ModelState.AddModelError("ImageFiles", "Yalniz Shekil fayli ola biler!");
					return View(phone);
				}
				if (img.Length > 3145728)
				{
					ModelState.AddModelError("ImageFiles", "Faylin ölçüsü max 3 mb ola biler!");
					return View(phone);
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
				Ekran = phone.Ekran,
				DaxiliYaddaş = phone.DaxiliYaddaş,
				OperativYaddaş = phone.OperativYaddaş,
				EsasKamera = phone.EsasKamera,
				OnKamera = phone.OnKamera,
				NüveSayı = phone.NüveSayı,
				ProsessorunAdı = phone.ProsessorunAdı,
				ProsessorunTezliyi = phone.ProsessorunTezliyi,
				EmeliyyatSistemi = phone.EmeliyyatSistemi,
				EmeliyyatSistemiVersiyası = phone.EmeliyyatSistemiVersiyası,
				İstehsalİli = phone.İstehsalİli,
				Çeki = phone.Çeki,
				İstehsalçı = phone.İstehsalçı

			};

			_dataContext.Features.Add(features);
			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Update(int id)
		{
			ViewBag.Brands = _dataContext.Brands.ToList();

			Product product = _dataContext.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == id);
			Features features = _dataContext.Features.FirstOrDefault(x => x.ProductId == product.Id);
			if (product == null || features is null) return NotFound();


			ViewBag.PosterImage = product.Images.Where(x => x.IsPoster == true).FirstOrDefault();
			ViewBag.Images = product.Images.Where(x => x.IsPoster == false).ToList();
			
			PhoneViewModel phoneVM = new PhoneViewModel
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
				ProductCount = product.ProdutCount,
				IsNew = product.IsNew,
				BrandId = product.BrandId,
				Ekran = features.Ekran,
				DaxiliYaddaş = features.DaxiliYaddaş,
				OperativYaddaş = features.OperativYaddaş,
				EsasKamera = features.EsasKamera,
				OnKamera = features.OnKamera,
				NüveSayı = features.NüveSayı,
				ProsessorunAdı = features.ProsessorunAdı,
				ProsessorunTezliyi = features.ProsessorunTezliyi,
				EmeliyyatSistemi = features.EmeliyyatSistemi,
				EmeliyyatSistemiVersiyası = features.EmeliyyatSistemiVersiyası,
				İstehsalİli = features.İstehsalİli,
				Çeki = features.Çeki,
				İstehsalçı = features.İstehsalçı
			};

			
			return View(phoneVM);
		}

		[HttpPost]
		public IActionResult Update(PhoneViewModel phone)
		{
			Product existProduct = _dataContext.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == phone.Id);

			ViewBag.Brands = _dataContext.Brands.ToList();

			ViewBag.PosterImage = existProduct.Images.FirstOrDefault(x => x.IsPoster == true);
			ViewBag.Images = existProduct.Images.Where(x => x.IsPoster == false).ToList();
			if (!ModelState.IsValid) return View();

			var existFeatures = _dataContext.Features.FirstOrDefault(x => x.ProductId == existProduct.Id);
			if (existProduct == null || existFeatures is null) return NotFound();

			

			if(phone.ImageIds != null)
			{
				existProduct.Images.RemoveAll(img => !phone.ImageIds.Contains(img.Id) && img.IsPoster == false);

			}

			if (phone.PosterImage != null)
			{
				if (phone.PosterImage.ContentType != "image/png" && phone.PosterImage.ContentType != "image/jpeg" && phone.PosterImage.ContentType != "image/jpg")
				{
					ModelState.AddModelError("PosterImage", "Yalniz Shekil fayli ola biler!");
					return View(phone);
				}
				if (phone.PosterImage.Length > 3145728)
				{
					ModelState.AddModelError("PosterImage", "Faylin ölçüsü max 3 mb ola biler!");
					return View(phone);
				}
				if(existProduct.Images != null)
				{
					var postImage = existProduct.Images.FirstOrDefault(x => x.IsPoster == true);
					FileManager.FileDelete(_env.WebRootPath, "uploads\\products", postImage.Image);
					existProduct.Images.Remove(postImage);
				}
				

				ProductImage productImage = new ProductImage
				{
					Product = existProduct,
					IsPoster = true,
					Image = FileManager.FileSave(_env.WebRootPath, "uploads\\products", phone.PosterImage)
				};

				_dataContext.ProductImages.Add(productImage);
			}

			if(phone.ImageFiles!= null)
			{
				foreach (var image in phone.ImageFiles)
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
			
			

			existProduct.Name = phone.Name;
			existProduct.SellPrice = phone.SellPrice;
			existProduct.CostPrice = phone.CostPrice;
			existProduct.DiscountPrice = phone.DiscountPrice;
			existProduct.IsAvailablity = phone.IsAvailablity;
			existProduct.IsFeatured = phone.IsFeatured;
			existProduct.Description = phone.Description;
			existProduct.IsNew = phone.IsNew;
			existProduct.BrandId = phone.BrandId;
			existProduct.ProdutCount = phone.ProductCount;

			existFeatures.Ekran = phone.Ekran;
			existFeatures.DaxiliYaddaş = phone.DaxiliYaddaş;
			existFeatures.OperativYaddaş = phone.OperativYaddaş;
			existFeatures.EsasKamera = phone.EsasKamera;
			existFeatures.OnKamera = phone.OnKamera;
			existFeatures.NüveSayı = phone.NüveSayı;
			existFeatures.ProsessorunAdı = phone.ProsessorunAdı;
			existFeatures.ProsessorunTezliyi = phone.ProsessorunTezliyi;
			existFeatures.EmeliyyatSistemi = phone.EmeliyyatSistemi;
			existFeatures.EmeliyyatSistemiVersiyası = phone.EmeliyyatSistemiVersiyası;
			existFeatures.İstehsalİli = phone.İstehsalİli;
			existFeatures.Çeki = phone.Çeki;
			existFeatures.İstehsalçı = phone.İstehsalçı;

			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));

		}

		public IActionResult Delete(int id)
		{
			Product deleteProduct = _dataContext.Products
									.Include(x=>x.Images)
									.FirstOrDefault(x => x.Id == id);
			if (deleteProduct == null) return NotFound();
			var deleteFeatures = _dataContext.Features.FirstOrDefault(x=>x.ProductId == id);

			var imagesList = _dataContext.ProductImages.Where(x => x.ProductId == deleteProduct.Id);
			foreach (var image in imagesList)
			{
				FileManager.FileDelete(_env.WebRootPath, "uploads\\products", image.Image);
				_dataContext.ProductImages.Remove(image);
			}
			if(deleteFeatures is not null)
			{
				_dataContext.Features.Remove(deleteFeatures);

			}
			_dataContext.Products.Remove(deleteProduct);
			_dataContext.SaveChanges();

			return Ok();
		}
	}
}
