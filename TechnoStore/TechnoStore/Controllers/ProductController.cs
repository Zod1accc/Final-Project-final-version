using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TechnoStore.Helpers;
using TechnoStore.Models;
using TechnoStore.Models.DataContext;
using TechnoStore.ViewModels;

namespace TechnoStore.Controllers
{

	public class ProductController : Controller
	{
		private DataContext _dataContext;
		private UserManager<AppUser> _userManager;

		public ProductController(DataContext dataContext,UserManager<AppUser> userManager)
		{
			_dataContext = dataContext;
			_userManager = userManager;
		}
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult SearchProducts(string? query,int page=1)
		{
			PaginatedList<Product> paginatedProducts = null;
			List<Product> results = null;
			PaginatedList<Product> paginated = null;

            if (query != null)
			{
				string[] keywords = query.ToLower().Split(' ');

				results = _dataContext.Products
					.Include(x => x.Images)
					.Include(x => x.Brand)
					.Include(x => x.Category)
					.AsEnumerable()
					.Where(p => keywords.All(k => p.Name.ToLower().Contains(k) || p.Description.ToLower().Contains(k) || p.Brand.Name.ToLower().Contains(k)))
					.ToList();

                
                paginated = new PaginatedList<Product>(results,results.Count,page,12);
            }
			
			
			ViewData["Searach"] = query;

			return View(paginated);
		}

		public async Task<IActionResult> Shop(int page=1)
		{
			var query = _dataContext.Products.AsQueryable();
			
			ShopViewModel shopVM = new ShopViewModel
			{
				Products = _dataContext.Products
											.Include(x => x.Images)
											.Include(x => x.Brand)
											.Include(x => x.Category).ToList(),

				PaginatedProducts = PaginatedList<Product>.Create(query,12, page),

				IsFeatured = _dataContext.Products
											.Where(x => x.IsFeatured == true)
											.Include(x => x.Images)
											.Take(3)
											.ToList(),
				IsNew = _dataContext.Products
											.Where(x => x.IsNew == true)
											.Include(x => x.Images)
											.Take(3)
											.ToList(),
				BestSeller = await _dataContext.Products
											.OrderByDescending(p => p.SellCount)
											.Include(x=>x.Images)
											.Take(3)  
											.ToListAsync()
			};
			return View(shopVM);
		}

		public async Task<IActionResult> ShopFilter(int page = 1)
		{
			var query = _dataContext.Products.AsQueryable();

			ShopViewModel shopVM = new ShopViewModel
			{
				Products = _dataContext.Products
											.Include(x => x.Images)
											.Include(x => x.Brand)
											.Include(x => x.Category).ToList(),

				PaginatedProducts = PaginatedList<Product>.Create(query, 12, page),

				IsFeatured = _dataContext.Products
											.Where(x => x.IsFeatured == true)
											.Include(x => x.Images)
											.Take(3)
											.ToList(),
				IsNew = _dataContext.Products
											.Where(x => x.IsNew == true)
											.Include(x => x.Images)
											.Take(3)
											.ToList(),
				BestSeller = await _dataContext.Products
											.OrderByDescending(p => p.SellCount)
											.Include(x => x.Images)
											.Take(3)
											.ToListAsync()
			};
			return View(shopVM);
		}

		public async Task<IActionResult> Filter(int id)
		{
			//id == 1 ---> Price
			List<Product> products = null;
			if(id == 1)
			{
				products = await _dataContext.Products
									.OrderByDescending(x => x.SellPrice)
									.Include(x => x.Images)
									.Include(x => x.Category)
									.ToListAsync();
			}
			else if(id == 2)
			{
				products = await _dataContext.Products
									.OrderByDescending(x => x.Viewed)
									.Include(x => x.Images)
									.Include(x => x.Category)
									.ToListAsync();
			}
			
			return PartialView("_shopFilter",products);
		}

		public async Task<IActionResult> GetProductOfCategory(int id,int page=1)
		{
			Category category = _dataContext.Categories.FirstOrDefault(x => x.Id == id);
			if(category== null) return NotFound();

			var query = _dataContext.Products
											.Include(x => x.Category)
											.Include(x => x.Images)
											.Where(x => x.CategoryId == id).AsQueryable();

			
			ShopViewModel shopVM = new ShopViewModel
			{

				PaginatedProducts = PaginatedList<Product>.Create(query, 12, page),

				IsFeatured = _dataContext.Products
											.Where(x => x.IsFeatured == true)
											.Include(x => x.Images)
											.Take(3)
											.ToList(),
				IsNew = _dataContext.Products
											.Where(x => x.IsNew == true)
											.Include(x => x.Images)
											.Take(3)
											.ToList(),
				BestSeller = await _dataContext.Products
											.OrderByDescending(p => p.SellCount)
											.Include(x => x.Images)
											.Take(3)
											.ToListAsync(),
			};

			return View(shopVM);
		}

		public IActionResult Detail(int id)
		{
			Product product = _dataContext.Products
							       .Include(x=>x.Category)
								   .Include(x=>x.Brand)
								   .Include(x=>x.Images).FirstOrDefault(x => x.Id == id);
			Features features = _dataContext.Features.FirstOrDefault(x=>x.ProductId == id);
			if (product == null) return View("Error");

			ProductDetailViewModel productDetailVM = new ProductDetailViewModel
			{
				Product = product,
				RelatedProduct = _dataContext.Products
										.Include(x=>x.Category)
										.Include(x=>x.Brand)
										.Include(x=>x.Images)
										.Where(x => x.CategoryId == product.CategoryId /*&& x.BrandId == product.BrandId*/).ToList(),
				keyValuePairs =  features.GetNotNullProperties()
			};

			product.Viewed++;
			_dataContext.SaveChanges();

			return View(productDetailVM);
			
		}

		public async Task<IActionResult> Checkout()
		{
			List<BasketViewModel> basketItems = new List<BasketViewModel>();
			List<CheckOutViewModel> checkOutVMList = new List<CheckOutViewModel>();
			CheckOutViewModel checkOutVM = null;
			List<BasketItem> userBasketItems = null;
			AppUser user = null;

			ViewBag.Countries = _dataContext.Countries.ToList();

			if (User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(User.Identity.Name);
			}

			if (user == null)
			{
				string basketStr = HttpContext.Request.Cookies["Basket"];

				if(basketStr != null)
				{
					basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketStr);

					foreach (var item in basketItems)
					{
						Product product = _dataContext.Products.FirstOrDefault(x=>x.Id == item.ProductId);
						if (product == null) return NotFound();

						checkOutVM = new CheckOutViewModel
						{
							Product = product,
							Count = item.Count
						};
						checkOutVMList.Add(checkOutVM);
					}
				}
			}

			else
			{
				userBasketItems = _dataContext.BasketItems.Include(x => x.Product).Where(x => x.AppUserId == user.Id && x.IsDeleted == false).ToList();

				if(userBasketItems is not null)
				{
					foreach (var item in userBasketItems)
					{

						checkOutVM = new CheckOutViewModel
						{
							Product = item.Product,
							Count = item.Count
						};
						checkOutVMList.Add(checkOutVM);
					}
				}
				
			}

			OrderViewModel orderVM = new OrderViewModel
			{
				checkoutItemViewModels = checkOutVMList,
				services = _dataContext.Services.ToList()

			};

			return View(orderVM);
		}

		[HttpPost]
		public async Task<IActionResult> Order(OrderViewModel orderVM)
		{
			List<BasketItem> userBasketItems = new List<BasketItem>();
			List<BasketViewModel> basketItems = new List<BasketViewModel>();
			ViewBag.Countries = _dataContext.Countries.ToList();

			if (!ModelState.IsValid) return RedirectToAction("Checkout", orderVM);
			double totalPrice = 0;
			double endPrice = 0;
			double zipCodeDiscount = 0;
            var zipCode = _dataContext.ZipCodes.FirstOrDefault(x => x.Code == orderVM.ZipCode);
			if(zipCode is not null)
			{
				zipCodeDiscount = zipCode.DiscountPrice;
			}
			//if (zipCode == null)
			//{
			//	ModelState.AddModelError("ZipCode", "ZipCode not found!");
			//	return View("Checkout");
			//}

			//zipCodeDiscount = zipCode.DiscountPrice;


			AppUser user = null;
			if (User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(User.Identity.Name);
			}

			string basketStrCheck = HttpContext.Request.Cookies["Basket"];
			var userItemsCheck = _dataContext.BasketItems.ToList();

			var checkBasket = Basket.CheckBasket(user, userItemsCheck, basketStrCheck);
			if (checkBasket == 0) return RedirectToAction("Checkout", orderVM);

			Order newOrder = new Order
			{
				Fullname = orderVM.Fullname,
				CountryId = orderVM.CountryId,
				Email = orderVM.Email,
				PhoneNumber = orderVM.PhoneNumber,
				Address1 = orderVM.Address1,
				Address2 = orderVM.Address2,
				City = orderVM.City,
				Note = orderVM?.Note,
				ZipCode = orderVM?.ZipCode,
				CreatedTime = DateTime.UtcNow.AddHours(4),
				AppUserId = user?.Id,
			};

			if (user == null)
			{
				string basketStr = HttpContext.Request.Cookies["Basket"];

				basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketStr);

				if (basketItems != null)
				{
					foreach (var item in basketItems)
					{
						if (item.Count > 0)
						{
                            Product product = _dataContext.Products.FirstOrDefault(x => x.Id == item.ProductId);
                            if (product is null) return NotFound();

                            product.SellCount += item.Count;
                            product.ProdutCount -= item.Count;

                            OrderItem orderItem = new OrderItem
                            {
                                Product = product,
                                ProductName = product.Name,
                                Count = item.Count,
                                CostPrice = product.CostPrice,
                                DiscountPrice = product.DiscountPrice,
                                SellPrice = product.SellPrice * (1 - (product.DiscountPrice / 100)),
                            };
                            totalPrice += orderItem.SellPrice * item.Count;
                            newOrder.OrderItems.Add(orderItem);

                            Response.Cookies.Append("Basket", "", new CookieOptions()
                            {
                                Expires = DateTime.Now.AddDays(-1)
                            });
                        }
						
					}
					endPrice = totalPrice - (totalPrice * zipCodeDiscount) / 100;
				}

			}
			else
			{
				userBasketItems = _dataContext.BasketItems.Where(x => x.AppUserId == user.Id && x.IsDeleted == false).ToList();

				if (userBasketItems != null)
				{
					foreach (var item in userBasketItems)
					{
						if (item.Count > 0)
						{
                            Product product = _dataContext.Products.FirstOrDefault(x => x.Id == item.ProductId);
                            if (product is null) return NotFound();

                            OrderItem orderItem = new OrderItem
                            {
                                Product = product,
                                ProductName = product.Name,
                                Count = item.Count,
                                CostPrice = product.CostPrice,
                                DiscountPrice = product.DiscountPrice,
                                SellPrice = product.SellPrice * (1 - (product.DiscountPrice / 100)),
                            };
                            item.IsDeleted = true;
                            totalPrice += orderItem.SellPrice * orderItem.Count;
                            newOrder.OrderItems.Add(orderItem);
                        }
						
					}
                    endPrice = totalPrice - (totalPrice * zipCodeDiscount) / 100;
                }
			}
			newOrder.TotalPrice = endPrice;

			_dataContext.Orders.Add(newOrder);
			_dataContext.SaveChanges();

			return RedirectToAction("index","home");
		}
		
		public async Task<IActionResult> AddToFavourites(int id)
		{
			List<FavoriteViewModel> favouriteItems = new List<FavoriteViewModel>();
			FavoriteViewModel favouriteItem = null;
			AppUser user = null;

			if (User.Identity.IsAuthenticated)
			{
				//user = _dataContext.Users.FirstOrDefault(x => x.NormalizedUserName == User.Identity.Name.ToString());
				user = await _userManager.FindByNameAsync(User.Identity.Name);
			}

			if (user == null)
			{
				string favouriteItemsStr = HttpContext.Request.Cookies["Favourite"];

				if (favouriteItemsStr != null)
				{
					favouriteItems = JsonConvert.DeserializeObject<List<FavoriteViewModel>>(favouriteItemsStr);

					favouriteItem = favouriteItems.FirstOrDefault(x => x.ProductId == id);
					if (favouriteItem != null) favouriteItems.Remove(favouriteItem);
					else
					{
						favouriteItem = new FavoriteViewModel
						{
							ProductId = id
						};
						favouriteItems.Add(favouriteItem);
					}

				}
				else
				{
					favouriteItem = new FavoriteViewModel
					{
						ProductId = id
					};
					favouriteItems.Add(favouriteItem);
				}

				favouriteItemsStr = JsonConvert.SerializeObject(favouriteItems);

				HttpContext.Response.Cookies.Append("Favourite", favouriteItemsStr);
			}
			else
			{
				var userFavourite = _dataContext.Favourites.FirstOrDefault(x=>x.ProductId == id && x.AppUserId ==user.Id);
				if (userFavourite != null) _dataContext.Favourites.Remove(userFavourite);
				else
				{
					Favourite newFavourite = new Favourite
					{
						ProductId = id,
						AppUserId = user.Id,
					};
					_dataContext.Favourites.Add(newFavourite);
				}
				_dataContext.SaveChanges();
			}


			return Ok();
		}

		public async Task<IActionResult> Wishlist()
		{
			List<FavoriteViewModel> favoriteItems = new List<FavoriteViewModel>();
			List<WishlistViewModel> wishlistItems = new List<WishlistViewModel>();
			WishlistViewModel wishlistItem = null;
			FavoriteViewModel favoriteItem = null;
			AppUser user = null;

			if (User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(User.Identity.Name);
			}

			if(user == null)
			{
				string favriteItemsStr = HttpContext.Request.Cookies["Favourite"];

				if (favriteItemsStr != null)
				{
					favoriteItems = JsonConvert.DeserializeObject<List<FavoriteViewModel>>(favriteItemsStr);

					foreach (var item in favoriteItems)
					{
						Product product = _dataContext.Products
											.Include(x => x.Images)
											.FirstOrDefault(x => x.Id == item.ProductId);
						if (product == null) return NotFound();

						wishlistItem = new WishlistViewModel
						{
							Product = product
						};
						wishlistItems.Add(wishlistItem);
					}
				}
			}
			else
			{
				var userFavourites = _dataContext.Favourites.Where(x => x.AppUserId == user.Id).ToList();

				foreach (var item in userFavourites)
				{
					Product product = _dataContext.Products
										.Include(x => x.Images)
										.FirstOrDefault(x=>x.Id == item.ProductId);
					if(product == null) return NotFound();

					wishlistItem = new WishlistViewModel
					{
						Product = product
					};
					wishlistItems.Add(wishlistItem);
				}
			}

			WishlistVM wishlistVM = new WishlistVM
			{
				WishlistItems = wishlistItems,
				Services = _dataContext.Services.ToList()
			};

			return View(wishlistVM);
		}

		public async Task<IActionResult> RemoveFavourite(int id)
		{
			List<FavoriteViewModel> favouriteItems = new List<FavoriteViewModel>();
			FavoriteViewModel favouriteItem = null;
			AppUser user = null;

			if (User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(User.Identity.Name);
			}

			if(user == null)
			{
				string favouritesStr = HttpContext.Request.Cookies["Favourite"];

				favouriteItems = JsonConvert.DeserializeObject<List<FavoriteViewModel>>(favouritesStr);

				if (favouriteItems is not null)
				{
					//Product product = _dataContext.Products.FirstOrDefault(x => x.Id == id);
					//if (product == null) return NotFound();

					favouriteItem = favouriteItems.FirstOrDefault(x => x.ProductId == id);
					if (favouriteItem is null) return NotFound();
					favouriteItems.Remove(favouriteItem);
				}

				favouritesStr = JsonConvert.SerializeObject(favouriteItems);

				HttpContext.Response.Cookies.Append("Favourite", favouritesStr);
			}
			else
			{
				var userFavoriteItems = _dataContext.Favourites.Where(x=>x.AppUserId == user.Id).ToList();

				if (userFavoriteItems != null)
				{
					var userFavouriteItem = userFavoriteItems.FirstOrDefault(x => x.ProductId == id);
					if (userFavouriteItem is null) return NotFound();

					_dataContext.Favourites.Remove(userFavouriteItem);
					_dataContext.SaveChanges();
				}
			}
			return RedirectToAction(nameof(Wishlist));
		}

		public async Task<IActionResult> SoppingCard()
		{
			List<BasketViewModel> basketItems = new List<BasketViewModel>();
			List<CheckOutViewModel> checkoutItems = new List<CheckOutViewModel>();
			List<BasketItem> userBasketItems = new List<BasketItem>();
			CheckOutViewModel checkoutItem = null;
			AppUser user = null;

			if (HttpContext.User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
			}

			if (user == null)
			{
				string basketItemsStr = HttpContext.Request.Cookies["Basket"];

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
				userBasketItems = _dataContext.BasketItems.Include(x => x.Product).Include(x => x.Product.Images).Where(x => x.AppUserId == user.Id && !x.IsDeleted).ToList();
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

			SoppingCardVM soppingCardVM = new SoppingCardVM
			{
				Services = _dataContext.Services.ToList(),
				BasketItems = checkoutItems
			};

			return View(soppingCardVM);
		}

		public async Task<IActionResult> DeleteShoppingItem(int id)
		{
			BasketViewModel basketItem = null;
			List<BasketItem> userBasketItems = new List<BasketItem>();
			BasketItem userBasketItem = null;
			List<BasketViewModel> basketItems = new List<BasketViewModel>();
			AppUser user = null;

			if (User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(User.Identity.Name);
			}

			if(user is null)
			{
                string basketItemStr = HttpContext.Request.Cookies["Basket"];

                if (basketItemStr != null)
                {
                    basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketItemStr);

                    basketItem = basketItems.FirstOrDefault(x => x.ProductId == id && x.Count >0);
                    if (basketItem == null) return NotFound();

					basketItems.Remove(basketItem);

					string basketItemsStr = JsonConvert.SerializeObject(basketItems);

					HttpContext.Response.Cookies.Append("Basket", basketItemsStr);
				}
            }
			else
			{
				userBasketItem = _dataContext.BasketItems.Where(x => x.AppUserId == user.Id && x.ProductId == id && x.IsDeleted == false).FirstOrDefault();
				if (userBasketItem is null) return NotFound();

				_dataContext.BasketItems.Remove(userBasketItem);
				_dataContext.SaveChanges();

			}

			return RedirectToAction(nameof(SoppingCard));

		}

		public async Task<IActionResult> Featured(int page=1)
		{
			var query = _dataContext.Products
											.Include(x => x.Category)
											.Include(x => x.Images)
											.Where(x => x.IsFeatured == true).AsQueryable();


			ShopViewModel shopVM = new ShopViewModel
			{

				PaginatedProducts = PaginatedList<Product>.Create(query, 12, page),

				IsFeatured = _dataContext.Products
											.Where(x => x.IsFeatured == true)
											.Include(x => x.Images)
											.Take(3)
											.ToList(),
				IsNew = _dataContext.Products
											.Where(x => x.IsNew == true)
											.Include(x => x.Images)
											.Take(3)
											.ToList(),
				BestSeller = await _dataContext.Products
											.OrderByDescending(p => p.SellCount)
											.Include(x => x.Images)
											.Take(3)
											.ToListAsync(),
			};

			return View(shopVM);
		}
	}
}
