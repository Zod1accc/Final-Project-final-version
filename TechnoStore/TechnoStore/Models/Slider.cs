using System.ComponentModel.DataAnnotations;
using TechnoStore.Areas.Manage.ViewModels;

namespace TechnoStore.Models
{
	public class Slider
	{
		public int Id { get; set; }
		[Required]
		[StringLength(maximumLength:40)]
		public string Header { get; set; }
		[Required]
		[StringLength(maximumLength:20)]
		public string Name1 { get; set; }
		
		[StringLength(maximumLength: 20)]
		public string? Name2 { get; set; }
		public string Image { get; set; }
		[Required]
		[StringLength(maximumLength: 250)]
		public string Description { get; set; }
		public double SellPrice { get; set; }
		public double DiscountPrice { get; set; }
		public double CostPrice { get; set; }
		public DateTime CreatedTime { get; set; }
		public DateTime? LastUpdate { get; set; }
		public string RedirectUrl { get; set; }
		[Required]
		[StringLength(maximumLength: 20)]
		public string RedirectUrlText { get; set; }

		//public static explicit operator Slider(SliderViewModel sliderVM)
		//{
		//	return new Slider
		//	{
		//		Name1 = sliderVM.Name1,
		//		Name2 = sliderVM.Name2,
		//		Description = sliderVM.Description,
		//		RedirectUrl = sliderVM.RedirectUrl,
		//		RedirectUrlText = sliderVM.RedirectUrlText,
		//		//Image = sliderVM.Image,
		//		SellPrice= sliderVM.SellPrice,
		//		DiscountPrice = sliderVM.DiscountPrice,
		//		Header = sliderVM.Header,
		//		CostPrice = sliderVM.CostPrice,

		//	};
		//}

		//public static implicit operator Slider(SliderViewModel sliderVM)
		//{
		//	return new Slider
		//	{
		//		Name1 = sliderVM.Name1,
		//		Name2 = sliderVM.Name2,
		//		Description = sliderVM.Description,
		//		RedirectUrl = sliderVM.RedirectUrl,
		//		RedirectUrlText = sliderVM.RedirectUrlText,
		//		SellPrice = sliderVM.SellPrice,
		//		CostPrice = sliderVM.CostPrice,
		//		Header = sliderVM.Header,
		//		DiscountPrice = sliderVM.DiscountPrice
		//	};
		//}
	}
}
