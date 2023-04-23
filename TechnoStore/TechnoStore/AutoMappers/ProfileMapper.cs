using AutoMapper;
using TechnoStore.Areas.Manage.ViewModels;
using TechnoStore.Models;

namespace TechnoStore.AutoMappers
{
	public class ProfileMapper:Profile
	{
		public ProfileMapper()
		{
			CreateMap<Slider,SliderViewModel>();
			CreateMap<SliderViewModel, Slider>();
		}
	}
}
