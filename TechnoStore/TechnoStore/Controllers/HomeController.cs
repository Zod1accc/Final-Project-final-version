using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using TechnoStore.Models;
using TechnoStore.Models.DataContext;
using TechnoStore.ViewModels;

namespace TechnoStore.Controllers
{
    public class HomeController : Controller
	{
		private DataContext _dataContext;
		private UserManager<AppUser> _userManage;

		public HomeController(DataContext dataContext, UserManager<AppUser> userManager)
		{
			_dataContext = dataContext;
			_userManage = userManager;
		}


		public async Task<IActionResult> Index()
		{
			HomeViewModel homeVM = new HomeViewModel
			{
				Sliders = await _dataContext.Sliders.ToListAsync(),
				Services = _dataContext.Services.ToList(),
				IsFeatured = _dataContext.Products
								.Include(x => x.Images)
								.Include(x => x.Category)
								.Where(x => x.IsFeatured == true).Take(4).ToList(),
				IsNew = _dataContext.Products
								.Include(x => x.Images)
								.Include(x => x.Category)
								.Where(x => x.IsNew == true).Take(4).ToList(),
				DiscountProduct = _dataContext.Products
								.Include(x => x.Images)
								.Include(x => x.Category)
								.Where(x => x.DiscountPrice > 0).Take(4).ToList(),
				Smartphones = _dataContext.Products
								.Include(x => x.Images)
								.Include(x => x.Category)
								.Where(x => x.Category.Name.ToLower() == "phone").Take(4).ToList(),
				Tvs = _dataContext.Products
								.Include(x => x.Images)
								.Include(x => x.Category)
								.Where(x => x.Category.Name.ToLower() == "tv").Take(4).ToList(),
				OurProducts = _dataContext.Products
								.Include(x => x.Images)
								.Include(x => x.Category)
								.Take(12).ToList(),
				HomePageBanners = _dataContext.HomePageBanners.ToList(),

				BestSeller = await _dataContext.Products
								.OrderByDescending(p => p.SellCount)
								.Include(x => x.Images)
								.Include(x => x.Category)
								.Include(x => x.Brand)
								.Take(3)
								.ToListAsync(),

				MostViewed = await _dataContext.Products
								.OrderByDescending(p => p.Viewed)
								.Include(x => x.Images)
								.Include(x => x.Category)
								.Include(x => x.Brand)
								.Take(15)
								.ToListAsync(),
			};
			return View(homeVM);
		}


		public async Task<IActionResult> AddToBasket(int id)
		{
			if (!_dataContext.Products.Any(x => x.Id == id)) return NotFound();

			List<BasketViewModel> basketItems = new List<BasketViewModel>();
			BasketViewModel basketItem = null;
			AppUser user = null;
			string BasketStr = HttpContext.Request.Cookies["Basket"];

			if (User.Identity.IsAuthenticated)
			{
				user = await _userManage.FindByNameAsync(User.Identity.Name);
			}

			if (user == null)
			{
				if (BasketStr != null)
				{
					basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(BasketStr);

					basketItem = basketItems.FirstOrDefault(x => x.ProductId == id);

					if (basketItem != null) basketItem.Count++;
					else
					{
						basketItem = new BasketViewModel
						{
							ProductId = id,
							Count = 1
						};
						basketItems.Add(basketItem);
					}
				}
				else
				{
					basketItem = new BasketViewModel
					{
						ProductId = id,
						Count = 1
					};

					basketItems.Add(basketItem);
				}


				string basketItemsStr = JsonConvert.SerializeObject(basketItems);

				HttpContext.Response.Cookies.Append("Basket", basketItemsStr);
			}
			else
			{
				BasketItem userBasketItems = _dataContext.BasketItems.FirstOrDefault(b => b.AppUserId == user.Id && b.ProductId == id && b.IsDeleted == false);
				if (userBasketItems != null) userBasketItems.Count++;
				else
				{
					var userbasketItem = new BasketItem
					{
						ProductId = id,
						AppUserId = user.Id,
						Count = 1,
						IsDeleted = false
					};
					_dataContext.BasketItems.Add(userbasketItem);
				}

				_dataContext.SaveChanges();

			}


			return Ok();
		}

		public async Task<IActionResult> GetBasket()
		{
			List<BasketViewModel> basketItems = new List<BasketViewModel>();
			List<BasketPartialViewModel> basketItemPartialVM = new List<BasketPartialViewModel>();
			List<BasketItem> userBasketItems = new List<BasketItem>();
			BasketPartialViewModel partialVM = null;
			AppUser user = null;

			if (User.Identity.IsAuthenticated)
			{
				user = await _userManage.FindByNameAsync(User.Identity.Name);
			}

			if (user == null)
			{
				string basketItemStr = HttpContext.Request.Cookies["Basket"];

				if (basketItemStr != null)
				{
					basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketItemStr);
				}

				foreach (var item in basketItems)
				{
					var product = _dataContext.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == item.ProductId);
					partialVM = new BasketPartialViewModel
					{
						Name = product.Name,
						Count = item.Count,
						Image = product.Images.Where(x => x.IsPoster == true).FirstOrDefault()?.Image,
						SellPrice = product.SellPrice,
						DiscountPrice = product.DiscountPrice,
						ProductId = product.Id,
					};
					basketItemPartialVM.Add(partialVM);
				}
			}
			else
			{
				userBasketItems = _dataContext.BasketItems.Where(x => x.AppUserId == user.Id && x.IsDeleted == false).ToList();

				foreach (var item in userBasketItems)
				{
					var product = _dataContext.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == item.ProductId);
					partialVM = new BasketPartialViewModel
					{
						Name = product.Name,
						Count = item.Count,
						Image = product.Images.Where(x => x.IsPoster == true).FirstOrDefault()?.Image,
						SellPrice = product.SellPrice,
						DiscountPrice = product.DiscountPrice,
						ProductId = product.Id,
					};
					basketItemPartialVM.Add(partialVM);
				}

			}


			return PartialView("_basketItemPartial", basketItemPartialVM);
		}

		public async Task<IActionResult> RemoveFromBasket(int id)
		{
			if (!_dataContext.Products.Any(x => x.Id == id)) return NotFound();
			List<BasketViewModel> basketItems = new List<BasketViewModel>();
			List<BasketViewModel> sentBasketItems = new List<BasketViewModel>();
			BasketViewModel sentbasketItem = null;
			BasketViewModel basketVM = null;
			List<BasketItem> userBasketItems = new List<BasketItem>();
			BasketItem userBasketItem = null;
			AppUser user = null;

			if (User.Identity.IsAuthenticated)
			{
				user = await _userManage.FindByNameAsync(User.Identity.Name);
			}

			if (user == null)
			{
				string basketItemStr = HttpContext.Request.Cookies["Basket"];

				if (basketItemStr != null)
				{
					basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketItemStr);

					BasketViewModel basketItem = basketItems.FirstOrDefault(x => x.ProductId == id);
					if (basketItem == null) return NotFound();
					if (basketItem.Count == 0)
					{
						basketItems.Remove(basketItem);
					}
					else
					{
						basketItem.Count--;
					}
					foreach (var item in basketItems)
					{
						sentBasketItems.Add(basketItem);
						
					}

					
				}
				basketItemStr = JsonConvert.SerializeObject(basketItems);
				HttpContext.Response.Cookies.Append("Basket", basketItemStr);

			}
			else
			{
				userBasketItems = _dataContext.BasketItems.Where(x => x.AppUserId == user.Id && x.IsDeleted == false).ToList();

				userBasketItem = userBasketItems.FirstOrDefault(x => x.ProductId == id);

				if (userBasketItem.Count == 0) _dataContext.BasketItems.Remove(userBasketItem);
				else
				{
					userBasketItem.Count--;
				}

				

				_dataContext.SaveChanges();

				var userEditBasketItems = _dataContext.BasketItems.Where(x => x.AppUserId == user.Id && x.IsDeleted == false).ToList();
				basketVM = new BasketViewModel
				{
					ProductId = userBasketItem.ProductId,
					Count = userBasketItem.Count
				};
				foreach (var item in userEditBasketItems)
				{
					sentBasketItems.Add(basketVM);
					
				}

				
			}

			return Json(sentBasketItems);
		}

		public IActionResult CleanBasket()
		{
			List<BasketViewModel> basketItems = new List<BasketViewModel>();


			string basketItemStr = HttpContext.Request.Cookies["Basket"];

			if (basketItemStr != null)
			{
				basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketItemStr);

				foreach (var item in basketItems)
				{
					if (item.Count == 0)
					{
						basketItems.Remove(item);
					}
				}

			}
			basketItemStr = JsonConvert.SerializeObject(basketItems);
			HttpContext.Response.Cookies.Append("Basket", basketItemStr);

			return Ok();
		}

		public IActionResult Contact()
		{
			ContactViewModel contactVM = new ContactViewModel
			{
				contactCenters = _dataContext.ContactCenters.ToList(),
				contactServices = _dataContext.ContactServices.ToList()
			};

			return View(contactVM);
		}

		[HttpPost]
		public async Task<IActionResult> Contact(ContactViewModel contact)
		{
			if (!ModelState.IsValid) return View(contact);

			AppUser user = null;

			if (User.Identity.IsAuthenticated)
			{
				user = await _userManage.FindByNameAsync(User.Identity.Name);
			}

			Contact newContact = new Contact
			{
				Fullname = contact.Fullname,
				Email = contact.Email,
				Subject = contact.Subject,
				Comment = contact.Comment,
				CreateDate = DateTime.Now,
				AppUserId = user?.Id
			};

			_dataContext.Contacts.Add(newContact);
			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> GetBasketCount()
		{
			List<BasketViewModel> basketItems = new List<BasketViewModel>();
			BasketViewModel basketVM = null;
			AppUser user = null;
			int count = 0;
			double price = 0;

			if (User.Identity.IsAuthenticated)
			{
				user = await _userManage.FindByNameAsync(User.Identity.Name);
			}

			if (user == null)
			{

				string basketItemStr = HttpContext.Request.Cookies["Basket"];

				if (basketItemStr != null)
				{
					basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketItemStr);
					foreach (var item in basketItems)
					{
						if (item.Count > 0)
						{
							var product = _dataContext.Products.FirstOrDefault(x => x.Id == item.ProductId);
							if (product == null) return View();

							count += item.Count;
							price += (product.SellPrice - ((product.SellPrice * product.DiscountPrice) / 100)) * item.Count;
						}

					}


				}
			}
			else
			{
				List<BasketItem> userBasketItems = _dataContext.BasketItems.Where(x => x.AppUserId == user.Id && x.IsDeleted == false && x.Count > 0).ToList();

				foreach (var item in userBasketItems)
				{
					if (item.Count > 0)
					{
						var product = _dataContext.Products.FirstOrDefault(x => x.Id == item.ProductId);
						if (product == null) return View();

						count += item.Count;
						price += (product.SellPrice - ((product.SellPrice * product.DiscountPrice) / 100)) * item.Count;
					}

				}

			}

			SentBasketVM sentBasketVM = new SentBasketVM
			{
				Count = count,
				Price = price
			};

			return Json(sentBasketVM);
		}

		public IActionResult Support()
		{
			var supports = _dataContext.Supports.ToList();

			return View(supports);
		}

		public IActionResult ChangeLanguage(string culture)
		{
			Response.Cookies.Append(
				CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
				new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(30) }
				);
			string returnUrl = Request.Headers["Referer"].ToString();
			return Redirect(returnUrl);
		}
		
		public IActionResult Deneme()
		{
			return View();
		}

		public TimeSpan DiscountDate()
		{
			DateTime discountDate = new DateTime(2023, 4, 15);
			var endTime = discountDate - DateTime.UtcNow;
			return endTime;
			//return Content(JsonConvert.SerializeObject(discountDate, new IsoDateTimeConverter()));
		}

		public static Dictionary<string, int> Countdown(DateTime endDate)
		{
			Dictionary<string, int> countdown = new Dictionary<string, int>();

			TimeSpan timeLeft = endDate - DateTime.UtcNow;

			if (timeLeft > TimeSpan.Zero)
			{
				countdown.Add("days", timeLeft.Days);
				countdown.Add("hours", timeLeft.Hours);
				countdown.Add("minutes", timeLeft.Minutes);
				countdown.Add("seconds", timeLeft.Seconds);
			}

			return countdown;
		}
	}
}