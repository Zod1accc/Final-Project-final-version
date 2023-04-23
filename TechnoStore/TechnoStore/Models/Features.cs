using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace TechnoStore.Models
{
	public class Features
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public int? ColorId { get; set; }
		//Telephone
		[StringLength(maximumLength:50)]
		public string? Ekran { get; set; }
		[StringLength(maximumLength:50)]
		public string? DaxiliYaddaş { get; set; }
		[StringLength(maximumLength:50)]
		public string? OperativYaddaş { get; set; }
		[StringLength(maximumLength:50)]
		public string? EsasKamera { get; set; }
		[StringLength(maximumLength:50)]
		public string? OnKamera { get; set; }
		public int? NüveSayı { get; set; }
		[StringLength(maximumLength: 50)]
		public string? ProsessorunAdı { get; set; }
		[StringLength(maximumLength: 50)]
		public string? ProsessorunTezliyi { get; set; }
		[StringLength(maximumLength: 50)]
		public string? EmeliyyatSistemi { get; set; }
		[StringLength(maximumLength: 50)]
		public string? EmeliyyatSistemiVersiyası { get; set; }
		public int? İstehsalİli { get; set; }
		public double? Çeki { get; set; }
		[StringLength(maximumLength: 50)]
		public string? İstehsalçı { get; set; }

		//TV
		[StringLength(maximumLength: 50)]
		public string? EkranNovu { get; set; }
		[StringLength(maximumLength: 50)]
		public string? EkranIcazesi { get; set; }
		[StringLength(maximumLength: 50)]
		public string? Tezlik { get; set; }
		[StringLength(maximumLength: 50)]
		public string? SesSistemi { get; set; }
		[StringLength(maximumLength: 50)]
		public string? IşığınNövü { get; set; }
		public double? Cheki { get; set; }
		[StringLength(maximumLength: 50)]
		public string? Olchu { get; set; }
		[StringLength(maximumLength: 50)]
		public string? İstehsalcı { get; set; }
		public Product Product { get; set; }
		public Color? Color { get; set; }

		public Dictionary<string, string> GetNotNullProperties()
		{
			var properties = GetType().GetProperties()
				.Where(p => p.Name != "Id" &&  p.Name != "ProductId" && p.Name != "ColorId" && (p.PropertyType == typeof(string) || p.PropertyType == typeof(int) || p.PropertyType == typeof(double)))
				.Where(p => p.GetValue(this) != null)
				.ToDictionary(p => p.Name, p => p.GetValue(this).ToString());

			return properties;
		}

	}
}
