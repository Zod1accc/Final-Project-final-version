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
	public class DashboardController : Controller
	{
		private DataContext _dataContext;

		public DashboardController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public async Task<IActionResult> Index(int page=1,int page2=1,int page3=1)
		{
			var products = _dataContext.Products.ToList();
			double price = 0;
			double totalPrice = 0;

			foreach (var item in products)
			{
				price = item.SellPrice - (item.SellPrice * item.DiscountPrice) / 100;
				totalPrice += price;
			}

			var orderQuery = _dataContext.Orders.AsQueryable();

			var bestsellers = await _dataContext.Products
								.OrderByDescending(p => p.SellCount)
								.Include(x => x.Images)
								.Include(x => x.Category)
								.Include(x => x.Brand)
								.Take(5)
								.ToListAsync();

			var mostViews = await _dataContext.Products
								.OrderByDescending(p => p.Viewed)
								.Include(x => x.Images)
								.Include(x => x.Category)
								.Include(x => x.Brand)
								.Take(15)
								.ToListAsync();
			DashboardViewModel dashboardController = new DashboardViewModel
			{
				Price = totalPrice,
				Products = _dataContext.Products.ToList(),

				Orders = _dataContext.Orders.ToList(),

				PaginatedOrder = PaginatedList<Order>.Create(orderQuery, 5, page),

				Users = _dataContext.Users.ToList(),

				BestSellers = await _dataContext.Products
								.OrderByDescending(p => p.SellCount)
								.Include(x => x.Images)
								.Include(x => x.Category)
								.Include(x => x.Brand)
								.Take(5)
								.ToListAsync(),

				PaginatedBestseller = new PaginatedList<Product>(bestsellers, bestsellers.Count, page2, 5),

				MostViewed = await _dataContext.Products
								.OrderByDescending(p => p.Viewed)
								.Include(x => x.Images)
								.Include(x => x.Category)
								.Include(x => x.Brand)
								.Take(5)
								.ToListAsync(),
				PaginatedMostView = new PaginatedList<Product>(mostViews, mostViews.Count, page3, 5),
			};

			return View(dashboardController);
		}

		public IActionResult OrderItem(int id)
		{
			var order = _dataContext.Orders
									.Include(x => x.AppUser)
									.Include(x => x.Country)
									.Include(x=>x.OrderItems)
										.ThenInclude(x=>x.Product.Images)
									.FirstOrDefault(x => x.Id == id);
			if (order == null) return NotFound();

			return View(order);
		}
	}
}
