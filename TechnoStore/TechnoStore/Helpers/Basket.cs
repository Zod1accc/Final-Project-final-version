using Newtonsoft.Json;
using TechnoStore.Models;
using TechnoStore.ViewModels;

namespace TechnoStore.Helpers
{
	public static class Basket
	{
		public static int CheckBasket(AppUser user,List<BasketItem> userBasketItems,string basketItemStr)
		{
			List<BasketViewModel> basketItems = new List<BasketViewModel>();
			List<BasketPartialViewModel> basketItemPartialVM = new List<BasketPartialViewModel>();
			//List<BasketItem> userBasketItems = new List<BasketItem>();
			BasketPartialViewModel partialVM = null;
			int check = 0;


			if (user == null)
			{
				//string basketItemStr = HttpContext.Request.Cookies["Basket"];

				if (basketItemStr != null)
				{
					basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketItemStr);
				}

				foreach (var item in basketItems)
				{
					if (item.Count > 0)
					{
						check++;
					}
				}
			}
			else
			{
				if(userBasketItems is not null)
				{
					foreach (var item in userBasketItems)
					{
						if (item.Count > 0)
						{
							check++;
						}
					}
				}
			}
			return check;
		}
	}
}
