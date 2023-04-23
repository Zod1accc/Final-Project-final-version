using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using TechnoStore.Areas.Manage.ViewModels;
using TechnoStore.Helpers;
using TechnoStore.Models;
using TechnoStore.Models.DataContext;
using TechnoStore.ViewModels;

namespace TechnoStore.Services
{
	public class LayoutServis
	{
		private DataContext _dataContext;
		private UserManager<AppUser> _userManager;
		private IHttpContextAccessor _httpContextAccessor;
		private IStringLocalizer<SharedResource> _localizer;

		public LayoutServis(DataContext dataContext,UserManager<AppUser> userManager,IHttpContextAccessor httpContextAccessor,IStringLocalizer<SharedResource> localizer)
		{
			_dataContext = dataContext;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
			_localizer = localizer;
		}
		public async Task<List<CheckOutViewModel>> GetBasket()
		{
			List<BasketViewModel> basketItems = new List<BasketViewModel>();
			List<CheckOutViewModel> checkoutItems = new List<CheckOutViewModel>();
			List<BasketItem> userBasketItems = new List<BasketItem>();
			CheckOutViewModel checkoutItem = null;
			AppUser user = null;

			if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
			}

			if (user == null)
			{
				string basketItemsStr = _httpContextAccessor.HttpContext.Request.Cookies["Basket"];

				if (basketItemsStr != null)
				{
					basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketItemsStr);
					foreach (var item in basketItems)
					{
						checkoutItem = new CheckOutViewModel
						{
							Product = _dataContext.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == item.ProductId),
							Count = item.Count
						};
						checkoutItems.Add(checkoutItem);
					}
				}
			}
			else
			{
				userBasketItems = _dataContext.BasketItems.Include(x=>x.Product).Include(x => x.Product.Images).Where(x => x.AppUserId == user.Id && !x.IsDeleted).ToList();
				if (userBasketItems is not null)
				{
					foreach (var item in userBasketItems)
					{
						checkoutItem = new CheckOutViewModel
						{
							Product = item.Product,
							Count = item.Count
						};
						checkoutItems.Add(checkoutItem);
					}
				}
			}

			return checkoutItems;

		}

		public Setting GetSettings()
		{
			var settings =  _dataContext.Settings.FirstOrDefault();

			return settings;
		}

		public List<Category> GetCategories()
		{
			var categories = _dataContext.Categories.ToList();

			return categories;
		}

		public async Task<UserDetailViewModel> GetUser()
		{
			AppUser user = null;
			UserDetailViewModel userDetailVM = null;
			if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
				var roles = _dataContext.UserRoles.FirstOrDefault(x => x.UserId == user.Id);
				//if (roles == null)

				var role = _dataContext.Roles.FirstOrDefault(x => x.Id == roles.RoleId);


				userDetailVM = new UserDetailViewModel
				{
					User = user,
					Role = role.Name
				};
			}

			

			return userDetailVM;
		}

		public string GetRoleForUserId(string id)
		{
			var userRoles = _dataContext.UserRoles.FirstOrDefault(x => x.UserId == id);
			var role = _dataContext.Roles.FirstOrDefault(x => x.Id == userRoles.RoleId);

			return role.Name;
		}

		public async Task<SentBasketVM> SentBasket()
		{
			List<BasketViewModel> basketItems = new List<BasketViewModel>();
			BasketViewModel basketVM = null;
			AppUser user = null;
			int count = 0;
			double price = 0;

			if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
			}

			if (user == null)
			{

				string basketItemStr = _httpContextAccessor.HttpContext.Request.Cookies["Basket"];

				if (basketItemStr != null)
				{
					basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketItemStr);
					foreach (var item in basketItems)
					{
						var product = _dataContext.Products.FirstOrDefault(x => x.Id == item.ProductId);
						//if (product == null) return View();

						count += item.Count;
						price += (product.SellPrice - ((product.SellPrice * product.DiscountPrice) / 100)) * item.Count;
					}


				}
			}
			else
			{
				List<BasketItem> userBasketItems = _dataContext.BasketItems.Where(x => x.AppUserId == user.Id && x.IsDeleted == false).ToList();

				foreach (var item in userBasketItems)
				{
					var product = _dataContext.Products.FirstOrDefault(x => x.Id == item.ProductId);

					count += item.Count;
					price += (product.SellPrice - ((product.SellPrice * product.DiscountPrice) / 100)) * item.Count;
				}

			}

			SentBasketVM sentBasketVM = new SentBasketVM
			{
				Count = count,
				Price = price
			};
			
			return sentBasketVM;
		}

		public WidgetViewModel GetWidgets()
		{
			var widgetTitles = _dataContext.WidgetTitles.ToList();

			var widgetTitle1 = widgetTitles[0];
			var widgetTitle2 = widgetTitles[1];

			var widgetTitle1Items = _dataContext.WidgetItems.Where(x=>x.WidgetTitle == widgetTitle1).ToList();
			var widgetTitle2Items = _dataContext.WidgetItems.Where(x=>x.WidgetTitle == widgetTitle2).ToList();

			WidgetViewModel widgetVM = new WidgetViewModel
			{
				WidgetTitle1 = widgetTitle1,
				WidgetTitle2 = widgetTitle2,
				WidgetTitle1Items = widgetTitle1Items,
				WidgetTitle2Items = widgetTitle2Items
			};

			return widgetVM;

		}

		
	}
}
