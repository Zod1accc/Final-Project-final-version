using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechnoStore.Helpers;
using TechnoStore.Models;
using TechnoStore.Models.DataContext;
using TechnoStore.ViewModels;

namespace TechnoStore.Areas.Manage.Controllers
{
	[Area("Manage")]
	[Authorize(Roles = "SuperAdmin,Admin")]
	public class UsersController : Controller
	{
		private DataContext _dataContext;
		private UserManager<AppUser> _userManager;

		public UsersController(DataContext dataContext,UserManager<AppUser> userManager)
		{
			_dataContext = dataContext;
			_userManager = userManager;
		}
		public IActionResult Index(string? searchString,int page=1)
		{
			List<AppUser> results = null;
			PaginatedList<AppUser> paginated = null;
			//var usersRole = _dataContext.Users.ToList();
			var query = _dataContext.Users.AsQueryable();

			if (searchString != null)
			{
				string[] keywords = searchString.ToLower().Split(' ');

				results = _dataContext.Users
					.AsEnumerable()
					.Where(p => keywords.All(k => p.UserName.ToLower().Contains(k) || p.Fullname.ToLower().Contains(k) || p.Email.ToLower().Contains(k)))
					.ToList();
	

				paginated = new PaginatedList<AppUser>(results, results.Count, page, 12);
			}
			else
			{
				paginated = PaginatedList<AppUser>.Create(query, 10, page);
			}

			ViewData["Searach"] = searchString;

			return View(paginated);
		}

		public async Task<IActionResult> Detail(string id)
		{
			ViewBag.Roles = _dataContext.Roles.ToList();

			List<Order> orders = _dataContext.Orders
									.Where(x => x.AppUserId == id).ToList();
			AppUser user = _dataContext.Users.FirstOrDefault(x => x.Id == id);
			DetailViewModel detailViewModel = new DetailViewModel
			{
				Orders = orders,
				User = user,
				IsBanned = user.IsBanned,
				AppUserId = id
			};

			return View(detailViewModel);
		}

		public async Task<IActionResult> Edit(DetailViewModel detailVM)
		{
			ViewBag.Roles = _dataContext.Roles.ToList();

			if (!ModelState.IsValid) return View("Detail");

			AppUser existUser = _dataContext.Users.FirstOrDefault(x => x.Id == detailVM.AppUserId);
			if (existUser == null) return NotFound();

			var oldrole = _dataContext.UserRoles.FirstOrDefault(x => x.UserId == detailVM.AppUserId);
			if (oldrole == null) return NotFound();

			var role = _dataContext.Roles.FirstOrDefault(x => x.Id == detailVM.RoleId);
			if (role == null) return NotFound();

			if (oldrole.RoleId != role.Id)
			{
				_dataContext.UserRoles.Remove(oldrole);

				string roleName = role.Name;
				await _userManager.AddToRoleAsync(existUser, roleName);

			}

			existUser.IsBanned = detailVM.IsBanned;
			_dataContext.SaveChanges();


			return RedirectToAction(nameof(Index));
			

		}

		public IActionResult Delete(string id)
		{
			AppUser user = _dataContext.Users.FirstOrDefault(x => x.Id == id);
			if(user == null) return NotFound();


			_dataContext.Users.Remove(user);
			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}
	}
}
